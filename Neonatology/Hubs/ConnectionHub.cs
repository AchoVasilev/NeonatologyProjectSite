namespace Neonatology.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    using SignalRCoreWebRTC.Models;

    [Authorize]
    public class ConnectionHub : Hub<IConnectionHub>
    {
        private readonly List<User> Users;
        private readonly List<UserCall> UserCalls;
        private readonly List<CallOffer> CallOffers;
        private readonly IHubContext<ChatHub> chatHubContext;

        public ConnectionHub(List<User> users, List<UserCall> userCalls, List<CallOffer> callOffers, IHubContext<ChatHub> chatHubContext)
        {
            this.Users = users;
            this.UserCalls = userCalls;
            this.CallOffers = callOffers;
            this.chatHubContext = chatHubContext;
        }

        public async Task Join(string username)
        {
            // Add the new user
            this.Users.Add(new User
            {
                Username = username,
                ConnectionId = this.Context.ConnectionId
            });

            // Send down the new list to all clients
            await this.SendUserListUpdate();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Hang up any calls the user is in
            await this.HangUp(); // Gets the user from "Context" which is available in the whole hub

            // Remove the user
            this.Users.RemoveAll(u => u.ConnectionId == this.Context.ConnectionId);

            // Send down the new user list to all clients
            await this.SendUserListUpdate();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task CallUser(User targetConnectionId, string group)
        {
            var callingUser = this.Users.SingleOrDefault(u => u.ConnectionId == this.Context.ConnectionId);
            var targetUser = this.Users.SingleOrDefault(u => u.ConnectionId == targetConnectionId.ConnectionId);

            // Make sure the person we are trying to call is still here
            if (targetUser == null)
            {
                // If not, let the caller know
                await this.chatHubContext
                    .Clients
                    .Group(group)
                    .SendAsync("ReceiveMessage", callingUser.Username, $"Пропуснахте обаждане от {callingUser.Username}.");

                await this.Clients.Caller.CallDeclined(targetConnectionId, "Потребителят е напуснал.");
                return;
            }

            // And that they aren't already in a call
            if (this.GetUserCall(targetUser.ConnectionId) != null)
            {
                await this.chatHubContext
                    .Clients
                    .Group(group)
                    .SendAsync("ReceiveMessage", callingUser.Username, $"Пропуснахте обаждане от {callingUser.Username}.");

                await this.Clients.Caller.CallDeclined(targetConnectionId, string.Format("{0} провежда разговор.", targetUser.Username));
                return;
            }

            // They are here, so tell them someone wants to talk
            await this.Clients.Client(targetConnectionId.ConnectionId).IncomingCall(callingUser);

            // Create an offer
            this.CallOffers.Add(new CallOffer
            {
                Caller = callingUser,
                Callee = targetUser
            });
        }

        public async Task AnswerCall(bool acceptCall, User targetConnectionId, string group)
        {
            var callingUser = this.Users.SingleOrDefault(u => u.ConnectionId == this.Context.ConnectionId);
            var targetUser = this.Users.SingleOrDefault(u => u.ConnectionId == targetConnectionId.ConnectionId);

            // This can only happen if the server-side came down and clients were cleared, while the user
            // still held their browser session.
            if (callingUser == null)
            {
                return;
            }

            // Make sure the original caller has not left the page yet
            if (targetUser == null)
            {
                await this.chatHubContext
                    .Clients
                    .Group(group)
                    .SendAsync("ReceiveMessage", callingUser.Username, $"Пропуснахте обаждане от {callingUser.Username}.");

                await this.Clients.Caller.CallEnded(targetConnectionId, "Потребителят напусна.");
                return;
            }

            // Send a decline message if the callee said no
            if (acceptCall == false)
            {
                await this.Clients.Client(targetConnectionId.ConnectionId).CallDeclined(callingUser, string.Format("{0} не прие обаждането.", callingUser.Username));
                return;
            }

            // Make sure there is still an active offer.  If there isn't, then the other use hung up before the Callee answered.
            var offerCount = this.CallOffers.RemoveAll(c => c.Callee.ConnectionId == callingUser.ConnectionId
                                                            && c.Caller.ConnectionId == targetUser.ConnectionId);
            if (offerCount < 1)
            {
                await this.Clients.Caller.CallEnded(targetConnectionId, string.Format("{0} has already hung up.", targetUser.Username));
                return;
            }

            // And finally... make sure the user hasn't accepted another call already
            if (this.GetUserCall(targetUser.ConnectionId) != null)
            {
                // And that they aren't already in a call
                await this.Clients.Caller.CallDeclined(targetConnectionId, string.Format("{0} chose to accept someone elses call instead of yours :(", targetUser.Username));
                return;
            }

            // Remove all the other offers for the call initiator, in case they have multiple calls out
            this.CallOffers.RemoveAll(c => c.Caller.ConnectionId == targetUser.ConnectionId);

            // Create a new call to match these folks up
            this.UserCalls.Add(new UserCall
            {
                Users = new List<User> { callingUser, targetUser }
            });

            // Tell the original caller that the call was accepted
            await this.Clients.Client(targetConnectionId.ConnectionId).CallAccepted(callingUser);

            // Update the user list, since thes two are now in a call
            await this.SendUserListUpdate();
        }

        public async Task HangUp()
        {
            var callingUser = this.Users.SingleOrDefault(u => u.ConnectionId == this.Context.ConnectionId);

            if (callingUser == null)
            {
                return;
            }

            var currentCall = this.GetUserCall(callingUser.ConnectionId);

            // Send a hang up message to each user in the call, if there is one
            if (currentCall != null)
            {
                foreach (var user in currentCall.Users.Where(u => u.ConnectionId != callingUser.ConnectionId))
                {
                    await this.Clients.Client(user.ConnectionId).CallEnded(callingUser, string.Format("{0} затвори.", callingUser.Username));
                }

                // Remove the call from the list if there is only one (or none) person left.  This should
                // always trigger now, but will be useful when we implement conferencing.
                currentCall.Users.RemoveAll(u => u.ConnectionId == callingUser.ConnectionId);
                if (currentCall.Users.Count < 2)
                {
                    this.UserCalls.Remove(currentCall);
                }
            }

            // Remove all offers initiating from the caller
            this.CallOffers.RemoveAll(c => c.Caller.ConnectionId == callingUser.ConnectionId);

            await this.SendUserListUpdate();
        }

        // WebRTC Signal Handler
        public async Task SendSignal(string signal, string targetConnectionId)
        {
            var callingUser = this.Users.SingleOrDefault(u => u.ConnectionId == this.Context.ConnectionId);
            var targetUser = this.Users.SingleOrDefault(u => u.ConnectionId == targetConnectionId);

            // Make sure both users are valid
            if (callingUser == null || targetUser == null)
            {
                return;
            }

            // Make sure that the person sending the signal is in a call
            var userCall = this.GetUserCall(callingUser.ConnectionId);

            // ...and that the target is the one they are in a call with
            if (userCall != null && userCall.Users.Exists(u => u.ConnectionId == targetUser.ConnectionId))
            {
                // These folks are in a call together, let's let em talk WebRTC
                await this.Clients.Client(targetConnectionId).ReceiveSignal(callingUser, signal);
            }
        }

        private async Task SendUserListUpdate()
        {
            this.Users.ForEach(u => u.InCall = this.GetUserCall(u.ConnectionId) != null);
            await this.Clients.All.UpdateUserList(this.Users);
        }

        private UserCall GetUserCall(string connectionId)
        {
            var matchingCall = this.UserCalls.SingleOrDefault(uc => uc.Users.SingleOrDefault(u => u.ConnectionId == connectionId) != null);

            return matchingCall;
        }
    }

    public interface IConnectionHub
    {
        Task UpdateUserList(List<User> userList);

        Task CallAccepted(User acceptingUser);

        Task CallDeclined(User decliningUser, string reason);

        Task IncomingCall(User callingUser);

        Task ReceiveSignal(User signalingUser, string signal);

        Task CallEnded(User signalingUser, string signal);
    }
}
