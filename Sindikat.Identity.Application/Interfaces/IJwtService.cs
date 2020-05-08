using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IJwtService
    {
        Task<LoginSuccessDto> GenerateToken(User user);
        Task<LoginSuccessDto> RefreshToken(TokenForRefreshDto tokenForRefresh);
        ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime = false);
    }
}
