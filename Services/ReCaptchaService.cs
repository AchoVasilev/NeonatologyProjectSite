namespace Services;

using System.Net.Http;
using System.Threading.Tasks;
using Common;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using ViewModels.GoogleRecaptcha;

public class ReCaptchaService : ITransientService
{
    private readonly RecaptchaSetting recaptchaSetting;
    private const string VerifyLink = "https://www.google.com/recaptcha/api/siteverify";

    public ReCaptchaService(IOptions<RecaptchaSetting> recaptchaSetting)
    {
        this.recaptchaSetting = recaptchaSetting.Value;
    }

    public virtual async Task<RecaptchaResponse> ValidateResponse(string token)
    {
        var data = new RecaptchaData
        {
            ResponseToken = token,
            Secret = this.recaptchaSetting.SecretKey
        };

        var client = new HttpClient();

        var response = await client.GetStringAsync(VerifyLink + $"?secret={data.Secret}&response={data.ResponseToken}");
        var capturedResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(response);

        return capturedResponse;
    }
}