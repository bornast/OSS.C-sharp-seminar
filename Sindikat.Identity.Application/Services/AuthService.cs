using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        public AuthService(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IJwtService jwtFactory, 
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtFactory;
            _mapper = mapper;
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

    }
}
