using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Sindikat.Identity.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sindikat.Identity.Application.Interfaces;
using System.Linq;
using SystemClaim = System.Security.Claims.Claim;
using Sindikat.Identity.Application.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Sindikat.Identity.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository<RefreshToken> _repo;
        private readonly UserManager<User> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRefreshTokenService _refreshTokenService;        

        public JwtService(
            IConfiguration configuration, 
            IBaseRepository<RefreshToken> repo, 
            UserManager<User> userManager,
            TokenValidationParameters tokenValidationParameters,
            IRefreshTokenService refreshTokenService)
        {
            _configuration = configuration;
            _repo = repo;
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenService = refreshTokenService;
        }

        public string GetJtiFromToken(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        }

        public TimeSpan GetTokenExpiryTimeSpan(ClaimsPrincipal claimsPrincipal)
        {
            var expiryDateUnix = long.Parse(claimsPrincipal.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            return expiryDateTimeUtc - DateTime.UtcNow;
        }

        public string GetUserIdFromToken(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.NameId).Value;
        }

        public async Task<TokenAndRefreshTokenPairDto> GenerateTokenAndRefreshTokenPair(User user)
        {
            var claims = new List<SystemClaim>
            {
                new SystemClaim(JwtRegisteredClaimNames.Sub, user.UserName),
                new SystemClaim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new SystemClaim(JwtRegisteredClaimNames.NameId, user.Id),
                new SystemClaim(ClaimTypes.NameIdentifier, user.Id)
            };

            var roles = user.UserRoles.Select(x => x.Role).ToList();

            foreach (var role in roles)
            {
                claims.Add(new SystemClaim(ClaimTypes.Role, role.Name));
            }

            foreach (var userClaim in user.UserClaims)
            {
                claims.Add(new SystemClaim(userClaim.Claim.Name, userClaim.ClaimValue));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);            
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtExpireMinutes"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var refreshToken = await _refreshTokenService.CreateRefreshToken(token.Id, user, commit: true);

            return new TokenAndRefreshTokenPairDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }        

        public ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = validateLifetime;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
