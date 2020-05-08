using Sindikat.Identity.Domain.Entities;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task MarkAsUsed(string refreshToken, string jti, bool commit = false);
        Task MarkAsInvalid(string jti, bool commit = false);
        Task<RefreshToken> CreateRefreshToken(string jti, User user, bool commit = false);
    }
}
