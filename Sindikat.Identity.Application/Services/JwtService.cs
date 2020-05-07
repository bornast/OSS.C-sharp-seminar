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
        private readonly IBaseRepository<RefreshToken> _repo;
        private readonly UserManager<User> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public IConfiguration Configuration { get; set; }

        public JwtService(IConfiguration configuration, 
            IBaseRepository<RefreshToken> repo, 
            UserManager<User> userManager,
            TokenValidationParameters tokenValidationParameters)
        {
            Configuration = configuration;
            _repo = repo;
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<LoginSuccessDto> GenerateToken(User user)
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);            
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(Configuration["JwtExpireMinutes"]));

            var token = new JwtSecurityToken(
                Configuration["JwtIssuer"],
                Configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            _repo.Persist(refreshToken);

            await _repo.FlushAsync();

            return new LoginSuccessDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<LoginSuccessDto> RefreshToken(TokenForRefreshDto tokenForRefresh)
        {
            var validatedToken = GetPrincipalFromToken(tokenForRefresh.Token);
            
            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _repo.FirstOrDefaultAsync(x => x.Token == tokenForRefresh.RefreshToken && x.JwtId == jti);

            storedRefreshToken.Used = true;

            await _repo.FlushAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.NameId).Value);

            return await GenerateToken(user);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
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
