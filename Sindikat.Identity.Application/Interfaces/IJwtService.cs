using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IJwtService
    {
        string GetJtiFromToken(ClaimsPrincipal claimsPrincipal);
        TimeSpan GetTokenExpiryTimeSpan(ClaimsPrincipal claimsPrincipal);
        string GetUserIdFromToken(ClaimsPrincipal claimsPrincipal);
        Task<TokenAndRefreshTokenPairDto> GenerateTokenAndRefreshTokenPair(User user);
        ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime = false);
    }
}
