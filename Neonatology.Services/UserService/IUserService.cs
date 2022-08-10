namespace Neonatology.Services.UserService;

using System.Threading.Tasks;
using Common;
using Data.Models;

public interface IUserService : ITransientService
{
    Task<string> GetUserIdByDoctorIdAsync(string doctorId);

    Task<ApplicationUser> GetUserByIdAsync(string id);

    Task<ApplicationUser> FindByUserNameAsync(string username);
}