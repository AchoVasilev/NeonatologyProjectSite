namespace Services.UserService;

using System.Threading.Tasks;

using Data.Models;

public interface IUserService
{
    Task<string> GetUserIdByDoctorIdAsync(string doctorId);

    Task<ApplicationUser> GetUserByIdAsync(string id);

    Task<ApplicationUser> FindByUserNameAsync(string username);
}