using Sindikat.Identity.Application.Dtos;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IAuthValidatorService
    {
        void ValidateBeforeLogin(LoginDto userForLogin);
        Task ValidateBeforeRegistration(RegisterDto userForRegistration);
        Task ValidateBeforeTokenRefresh(TokenForRefreshDto tokenForRefresh);
    }
}
