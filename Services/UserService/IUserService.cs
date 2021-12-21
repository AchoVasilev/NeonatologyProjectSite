namespace Services.UserService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.Chat;

    public interface IUserService
    {
        Task<IEnumerable<ChatUserViewModel>> GetAllChatUsers();

        Task<ChatUserViewModel> GetChatUserById(string id);

        Task<string> GetUserIdByDoctorIdAsync(string doctorId);
    }
}
