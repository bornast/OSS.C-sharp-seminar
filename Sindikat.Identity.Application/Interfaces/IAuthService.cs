using Sindikat.Identity.Application.Dtos;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenAndRefreshTokenPairDto> Login(LoginDto userForLogin);
        Task Register(RegisterDto userForRegistration);
        Task<TokenAndRefreshTokenPairDto> RefreshToken(TokenForRefreshDto tokenForRefresh);
        Task SignOut(string token);
    }
}
