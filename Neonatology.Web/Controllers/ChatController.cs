﻿namespace Neonatology.Web.Controllers;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.ChatService;
using Services.DoctorService;
using Services.PatientService;
using Services.UserService;
using ViewModels.Chat;
using ViewModels.Patient;
using static Common.Constants.GlobalConstants;
using static Common.Constants.GlobalConstants.ChatConstants;
using static Common.Constants.WebConstants.RouteTemplates;
using static Common.Constants.WebConstants.HubsConstants;

[Authorize]
public class ChatController : BaseController
{
    private readonly IChatService chatService;
    private readonly IDoctorService doctorService;
    private readonly IUserService userService;
    private readonly IPatientService patientService;
    private readonly IHubContext<ChatHub> chatHub;

    public ChatController(
        IChatService chatService,
        IDoctorService doctorService,
        IUserService userService,
        IPatientService patientService,
        IHubContext<ChatHub> chatHub)
    {
        this.chatService = chatService;
        this.doctorService = doctorService;
        this.userService = userService;
        this.patientService = patientService;
        this.chatHub = chatHub;
    }

    public async Task<IActionResult> All([FromQuery] int page)
    {
        var currentUserId = this.User.GetId();
        var patientId = this.patientService.GetPatientIdByUserId(currentUserId);

        if (patientId != null)
        {
            return this.RedirectToAction(nameof(this.Consultation));
        }

        var doctorId = await this.doctorService.GetDoctorId();
        var doctorEmail = await this.doctorService.GetDoctorEmail(doctorId);

        var conversations = await this.chatService.GetAllMessages(currentUserId, page, ItemsPerPage);

        await this.ProcessConversations(conversations, currentUserId);

        var model = new ChatUserViewModel
        {
            Id = await this.userService.GetUserIdByDoctorIdAsync(doctorId),
            DoctorEmail = doctorEmail,
            ChatModels = conversations,
            PageNumber = page,
            ItemsPerPage = ItemsPerPage,
            ItemCount = conversations.Count
        };

        return this.View(model);
    }

    public async Task<IActionResult> Consultation()
    {
        var page = 1;
        var currentUserId = this.User.GetId();
        var conversations = await this.chatService.GetAllMessages(currentUserId, page, ItemsPerPage);

        await this.ProcessConversations(conversations, currentUserId);

        var doctorId = await this.doctorService.GetDoctorId();
        var doctorEmail = await this.doctorService.GetDoctorEmail(doctorId);

        var model = new ChatUserViewModel
        {
            Id = await this.userService.GetUserIdByDoctorIdAsync(doctorId),
            DoctorEmail = doctorEmail,
            ChatModels = conversations,
            PageNumber = page,
            ItemsPerPage = ItemsPerPage,
            ItemCount = conversations.Count,
            HasPaid = await this.patientService.HasPaid(currentUserId)
        };

        return this.View(model);
    }

    [Route(ChatWithUser)]
    public async Task<IActionResult> WithUser(string username, string group)
    {
        var currentUser = await this.userService.GetUserByIdAsync(this.User.GetId());
        var groupUsers = new List<string>() { currentUser.Email, username };
        var targetGroupName = group ?? string.Join(ChatGroupNameSeparator, groupUsers.OrderBy(x => x));

        var receiver = await this.userService.FindByUserNameAsync(username);
        var doctorReceiver = await this.doctorService.GetDoctorByUserId(receiver.Id);
        var patientReceiver = await this.patientService.GetPatientByUserId(receiver.Id);

        var sender = await this.userService.GetUserByIdAsync(this.HttpContext.User.GetId());
        var doctorSender = await this.doctorService.GetDoctorByUserId(sender.Id);
        var patientSender = await this.patientService.GetPatientByUserId(sender.Id);

        var receiverFullName = GetFullName(doctorReceiver, patientReceiver);

        var senderFullName = GetFullName(doctorSender, patientSender);

        var messages = await this.chatService.ExtractAllMessages(targetGroupName);
        var model = new PrivateChatViewModel
        {
            CurrentUser = sender,
            ToUser = receiver,
            ChatMessages = messages,
            GroupName = targetGroupName,
            ReceiverFullName = receiverFullName,
            SenderFullName = senderFullName,
            ReceiverEmail = receiver.Email,
            SenderEmail = sender.Email
        };

        if (patientSender != null && patientSender.Id == currentUser.PatientId && !patientSender.HasPaid)
        {
            model.HasPaid = false;
        }

        return this.View(model);
    }

    [HttpPost]
    [Route(ChatSendFiles)]
    public async Task<IActionResult> SendFiles(IList<IFormFile> files, string group, string toUsername,
        string fromUsername, string message)
    {
        //var currentUser = await this.userManager.GetUserAsync(this.User);

        //if (!isAvailableToChat)
        //{
        //    this.TempData["Error"] = ErrorMessages.NotAbleToChat;
        //    return this.RedirectToAction("Index", "Profile", new { Username = toUsername });
        //}

        var result = await this.chatService
            .SendMessageWitFilesToUser(files, group, toUsername, fromUsername, message);

        await this.chatHub
            .Clients
            .User(result.ReceiverId)
            .SendAsync(HubReceiveMessage, fromUsername, result.MessageContent);

        await this.chatHub
            .Clients
            .User(result.SenderId)
            .SendAsync(HubSendMessage, fromUsername, result.MessageContent);

        return new JsonResult(new { result.HaveFiles, result.HaveImages });
    }

    [HttpGet]
    [Route(ChatLoadMoreMessages)]
    public async Task<IActionResult> LoadMoreMessages(string username, string group, int? messagesSkipCount,
        string receiverFullname, string senderFullname)
    {
        var currentUser = await this.userService.GetUserByIdAsync(this.User.GetId());

        messagesSkipCount ??= 0;

        var data = await this.chatService
            .LoadMoreMessages(group, (int)messagesSkipCount, currentUser, receiverFullname, senderFullname);

        return new JsonResult(data);
    }

    private static string GetFullName(ViewModels.Doctor.DoctorProfileViewModel doctor, PatientViewModel patient)
    {
        return doctor != null ? $"Д-р {doctor.FullName}" : $"{patient.FirstName} {patient.LastName}";
    }

    private async Task ProcessConversations(IEnumerable<ChatConversationsViewModel> conversations, string currentUserId)
    {
        foreach (var user in conversations)
        {
            var lastMessage = await this.chatService.GetLastMessage(currentUserId, user.Id);
            var contentWithoutTags = Regex.Replace(lastMessage, "<.*?>", string.Empty);

            user.LastMessage = contentWithoutTags.Length < 487
                ? contentWithoutTags
                : $"{contentWithoutTags.Substring(0, 487)}...";

            user.LastMessageActivity = await this.chatService.GetLastActivityAsync(currentUserId, user.Id);

            var groupId = await this.chatService.GetGroupId(currentUserId, user.Id);
            user.GroupName = await this.chatService.GetGroupName(groupId);
        }
    }
}