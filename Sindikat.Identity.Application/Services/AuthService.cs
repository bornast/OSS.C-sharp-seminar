using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Common.Enums;
using Sindikat.Identity.Common.Exceptions;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly IBaseRepository<RefreshToken> _refreshTokenRepo;

        public AuthService(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IJwtService jwtFactory, 
            IMapper mapper,
            IDistributedCache distributedCache,
            IBaseRepository<RefreshToken> refreshTokenRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtFactory;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _refreshTokenRepo = refreshTokenRepo;
        }

        public async Task<LoginSuccessDto> Login(LoginDto userForLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(userForLogin.UserName, userForLogin.Password, false, false);

            if (!result.Succeeded)
                throw new UnauthorizedException();

            var user = _userManager.Users.SingleOrDefault(r => r.UserName == userForLogin.UserName);

            return await _jwtService.GenerateToken(user);
        }

        public async Task Register(RegisterDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);

            await _userManager.CreateAsync(user, userForRegistration.Password);

            await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(Roles), Roles.User));
        }

        public async Task<LoginSuccessDto> RefreshToken(TokenForRefreshDto tokenForRefresh)
        {
            return await _jwtService.RefreshToken(tokenForRefresh);
        }

        public async Task SignOut(string token)
        {
            // TODO: should this be in jwtService?
            var validatedToken = _jwtService.GetPrincipalFromToken(token);            

            var expiryDateUnix = long.Parse(validatedToken.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            var options = new DistributedCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes
               ((expiryDateTimeUtc - DateTime.UtcNow).TotalMinutes));

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var refreshTokens = await _refreshTokenRepo.WhereAsync(x => x.JwtId == jti && x.Invalidated == false && x.Used == false);

            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.Invalidated = true;
            }

            await _refreshTokenRepo.FlushAsync();

            await _distributedCache.SetAsync(jti, Encoding.UTF8.GetBytes(token), options);
        }

    }
}
