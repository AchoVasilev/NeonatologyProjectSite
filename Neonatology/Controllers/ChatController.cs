namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Mvc;

    using Services.DoctorService;
    using Services.MessageService;
    using Services.UserService;

    using ViewModels.Chat;

    public class ChatController : BaseController
    {
        private readonly IMessageService messageService;
        private readonly IUserService userService;
        private readonly IDoctorService doctorService;

        public ChatController(IMessageService messageService, IUserService userService, IDoctorService doctorService)
        {
            this.messageService = messageService;
            this.userService = userService;
            this.doctorService = doctorService;
        }

        public async Task<IActionResult> All()
        {
            var doctorId = await this.doctorService.GetDoctorId();
            var model = new ChatUserViewModel
            {
                Id = await this.userService.GetUserIdByDoctorIdAsync(doctorId)
            };

            return View(model);
        }

        public async Task<IActionResult> SendMessage()
        {
            var viewModel = new ChatSendMessageInputModel
            {
                Users = await this.userService.GetAllChatUsers()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatSendMessageInputModel model)
        {
            await this.messageService.CreateMessageAsync(model.Message, this.User.GetId(), model.ReceiverId);

            return RedirectToAction(nameof(WithUser), new { id = model.ReceiverId });
        }

        public async Task<IActionResult> WithUser(string id)
        {
            var currentUserId = this.User.GetId();

            var viewModel = new ChatWithUserViewModel
            {
                User = await this.userService.GetChatUserById(id),
                Messages = await this.messageService.GetAllWithUserAsync(currentUserId, id)
            };

            return View(viewModel);
        }
    }
}
