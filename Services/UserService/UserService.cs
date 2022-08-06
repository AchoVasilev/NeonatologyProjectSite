namespace Services.UserService;

using System.Linq;
using System.Threading.Tasks;

using Data;
using Data.Models;

using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly NeonatologyDbContext data;

    public UserService(NeonatologyDbContext data)
    {
        this.data = data;
    }

    public async Task<string> GetUserIdByDoctorIdAsync(string doctorId)
        => await this.data.Users
            .Where(x => x.DoctorId == doctorId && x.IsDeleted == false)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
        => await this.data.Users
            .Where(x => x.Id == id && x.IsDeleted == false)
            .Include(x => x.Image)
            .FirstOrDefaultAsync();

    public async Task<ApplicationUser> FindByUserNameAsync(string username)
        => await this.data.Users
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.UserName == username);
}