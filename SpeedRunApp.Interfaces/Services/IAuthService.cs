using SpeedRunApp.Model.JSON;
using System.Threading.Tasks;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IAuthService
    {
        Task<SocialTokenResponse> ValidateSocialToken(string accessToken, int socialAccountTypeID);
        Task<bool> ValidateGoogleRecaptcha(string token);
    }
}
