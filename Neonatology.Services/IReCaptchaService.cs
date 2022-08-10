namespace Neonatology.Services;

using System.Threading.Tasks;
using Common;
using ViewModels.GoogleRecaptcha;

public interface IReCaptchaService : ITransientService
{
    Task<RecaptchaResponse> ValidateResponse(string token);
}