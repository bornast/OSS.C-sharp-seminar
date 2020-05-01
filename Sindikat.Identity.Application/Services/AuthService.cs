using Microsoft.AspNetCore.Identity;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Common.Enums;
using Sindikat.Identity.Common.Exceptions;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtFactory _jwtFactory;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtFactory = jwtFactory;
        }

        public async Task<string> Login(LoginDto userForLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(userForLogin.UserName, userForLogin.Password, false, false);

            if (!result.Succeeded)
                throw new UnauthorizedException();

            var user = _userManager.Users.SingleOrDefault(r => r.UserName == userForLogin.UserName);

            return _jwtFactory.Generate(user).ToString();
        }

        public async Task Register(RegisterDto userForRegistration)
        {
            var user = new User
            {
                UserName = userForRegistration.UserName,
                Email = userForRegistration.Email
            };

            await _userManager.CreateAsync(user, userForRegistration.Password);

            await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(Roles), Roles.User));
        }
    }
}
