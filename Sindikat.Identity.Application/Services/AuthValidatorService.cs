using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Application.Validators;
using Sindikat.Identity.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class AuthValidatorService : BaseValidatorService, IAuthValidatorService
    {
        private readonly UserManager<User> _userManager;
        private readonly IBaseRepository<RefreshToken> _refreshTokenRepository;
        private readonly IJwtService _jwtService;

        public AuthValidatorService(UserManager<User> userManager, 
            IBaseRepository<RefreshToken> refreshTokenRepository,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
        }        

        public void ValidateBeforeLogin(LoginDto userForLogin)
        {
            var validator = new LoginDtoValidator();

            CheckValidationResults(validator.Validate(userForLogin));
        }

        public async Task ValidateBeforeRegistration(RegisterDto userForRegistration)
        {
            var validator = new RegisterDtoValidator();

            CheckValidationResults(validator.Validate(userForRegistration));

            var existingUserEmail = await _userManager.FindByEmailAsync(userForRegistration.Email);

            if (existingUserEmail != null)
                ThrowValidationError("Email", $"Email {userForRegistration.Email} already exists!");

            var existingUserUsername = await _userManager.FindByNameAsync(userForRegistration.UserName);

            if (existingUserUsername != null)
                ThrowValidationError("Username", $"Username {userForRegistration.UserName} already exists!");
        }

        public async Task ValidateBeforeTokenRefresh(TokenForRefreshDto tokenForRefresh)
        {
            var validatedToken = _jwtService.GetPrincipalFromToken(tokenForRefresh.Token);

            if (validatedToken == null)            
                ThrowValidationError("Token", "Invalid token!");

            var expiryDateUnix = long.Parse(validatedToken.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)            
                ThrowValidationError("Token", "Invalid token!");

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(x => x.Token == tokenForRefresh.RefreshToken && x.JwtId == jti);

            if (storedRefreshToken == null 
                || DateTime.UtcNow > storedRefreshToken.ExpiryDate 
                || storedRefreshToken.Invalidated
                || storedRefreshToken.Used)
                ThrowValidationError("Token", "Invalid token!");
        }

    }
}
