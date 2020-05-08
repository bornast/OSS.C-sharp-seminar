using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Common.Enums;
using Sindikat.Identity.Common.Exceptions;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ICacheService _cacheService;

        public AuthService(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IJwtService jwtFactory, 
            IMapper mapper,
            IRefreshTokenService refreshTokenService,
            ICacheService cacheService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtFactory;
            _mapper = mapper;
            _refreshTokenService = refreshTokenService;
            _cacheService = cacheService;
        }

        public async Task<TokenAndRefreshTokenPairDto> Login(LoginDto userForLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(userForLogin.UserName, userForLogin.Password, false, false);

            if (!result.Succeeded)
                throw new UnauthorizedException();

            var user = _userManager.Users.SingleOrDefault(r => r.UserName == userForLogin.UserName);

            return await _jwtService.GenerateTokenAndRefreshTokenPair(user);
        }

        public async Task Register(RegisterDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);

            await _userManager.CreateAsync(user, userForRegistration.Password);

            await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(Roles), Roles.User));
        }

        public async Task<TokenAndRefreshTokenPairDto> RefreshToken(TokenForRefreshDto tokenForRefresh)
        {
            var validatedToken = _jwtService.GetPrincipalFromToken(tokenForRefresh.Token);

            var jti = _jwtService.GetJtiFromToken(validatedToken);

            await _refreshTokenService.MarkAsUsed(tokenForRefresh.RefreshToken, jti, commit: false);

            var user = await _userManager.FindByIdAsync(_jwtService.GetUserIdFromToken(validatedToken));

            return await _jwtService.GenerateTokenAndRefreshTokenPair(user);
        }

        public async Task SignOut(string token)
        {
            var validatedToken = _jwtService.GetPrincipalFromToken(token);            
            
            var jti = _jwtService.GetJtiFromToken(validatedToken);

            await _cacheService.SetAsync(jti, token, _jwtService.GetTokenExpiryTimeSpan(validatedToken).TotalMinutes);

            await _refreshTokenService.MarkAsInvalid(jti, commit: true);
        }

    }
}
