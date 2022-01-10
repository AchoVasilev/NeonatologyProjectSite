namespace Neonatology.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Data.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Services.ChatService;
    using Services.DoctorService;
    using Services.PatientService;
    using Services.UserService;

    using ViewModels.Chat;
    using ViewModels.Patient;

    [Authorize]
    public class ChatController : BaseController
    {
        private readonly IChatService chatService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDoctorService doctorService;
        private readonly IUserService userService;
        private readonly IPatientService patientService;

        public ChatController(
            IChatService chatService,
            UserManager<ApplicationUser> userManager,
            IDoctorService doctorService,
            IUserService userService,
            IPatientService patientService)
        {
            this.chatService = chatService;
            this.userManager = userManager;
            this.doctorService = doctorService;
            this.userService = userService;
            this.patientService = patientService;
        }

        public async Task<IActionResult> All()
        {
            var doctorId = await this.doctorService.GetDoctorId();
            var model = new ChatUserViewModel
            {
                Id = await this.userService.GetUserIdByDoctorIdAsync(doctorId),
                Email = await this.doctorService.GetDoctorEmail(doctorId)
            };

            return View(model);
        }

        public async Task<IActionResult> WithUser(string username, string group)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var groupUsers = new List<string>() { currentUser.Email, username };
            var targetGroupName = group ?? string.Join(GlobalConstants.ChatGroupNameSeparator, groupUsers.OrderBy(x => x));

            var receiver = await this.userManager.FindByNameAsync(username);
            var doctorReceiver = await this.doctorService.GetDoctorByUserId(receiver.Id);
            var patientReceiver = await this.patientService.GetPatientByUserIdAsync(receiver.Id);

            var sender = await this.userManager.GetUserAsync(this.HttpContext.User);
            var doctorSender = await this.doctorService.GetDoctorByUserId(sender.Id);
            var patientSender = await this.patientService.GetPatientByUserIdAsync(sender.Id);

            var receiverFullName = GetFullName(doctorReceiver, patientReceiver);

            var senderFullName = GetFullName(doctorSender, patientSender);

            var messages = await this.chatService.ExtractAllMessages(targetGroupName);
            var model = new PrivateChatViewModel
            {
                FromUser = sender,
                ToUser = receiver,
                ChatMessages = messages,
                GroupName = targetGroupName,
                ReceiverFullName = receiverFullName,
                SenderFullName = senderFullName,
                ReceiverEmail = receiver.Email,
                SenderEmail = sender.Email
            };

            return this.View(model);
        }

        [HttpGet]
        [Route("[controller]/With/{username?}/Group/{group?}/LoadMoreMessages/{messagesSkipCount?}")]
        public async Task<IActionResult> LoadMoreMessages(string username, string group, int? messagesSkipCount)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (messagesSkipCount == null)
            {
                messagesSkipCount = 0;
            }

            ICollection<LoadMoreMessagesViewModel> data = await this.chatService
                .LoadMoreMessages(group, (int)messagesSkipCount, currentUser);

            return new JsonResult(data);
        }

        private static string GetFullName(ViewModels.Doctor.DoctorProfileViewModel doctor, PatientViewModel patient)
        {
            return doctor != null ?
                            $"Д-р {doctor.FullName}" :
                            $"{patient.FirstName} {patient.LastName}";
        }
    }
}
