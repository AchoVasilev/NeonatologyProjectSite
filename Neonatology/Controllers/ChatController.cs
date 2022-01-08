namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Mvc;

    using Services.DoctorService;
    using Services.MessageService;
    using Services.PatientService;
    using Services.PaymentService;
    using Services.UserService;

    using ViewModels.Chat;

    public class ChatController : BaseController
    {
        private readonly IMessageService messageService;
        private readonly IUserService userService;
        private readonly IDoctorService doctorService;
        private readonly IPaymentService paymentService;
        private readonly IPatientService patientService;

        public ChatController(
            IMessageService messageService,
            IUserService userService,
            IDoctorService doctorService,
            IPaymentService paymentService, 
            IPatientService patientService)
        {
            this.messageService = messageService;
            this.userService = userService;
            this.doctorService = doctorService;
            this.paymentService = paymentService;
            this.patientService = patientService;
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
            var isDoctor = await this.doctorService.UserIsDoctor(currentUserId);
            var patientId = await this.patientService.GetPatientIdByUserIdAsync(currentUserId);
            var hasPaid = await this.paymentService.PatientHasPaid(patientId);

            var viewModel = new ChatWithUserViewModel
            {
                User = await this.userService.GetChatUserById(id),
                Messages = await this.messageService.GetAllWithUserAsync(currentUserId, id),
                CanChat = hasPaid || isDoctor
            };

            return View(viewModel);
        }
    }
}
