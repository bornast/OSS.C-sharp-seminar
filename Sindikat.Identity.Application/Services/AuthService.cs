using Microsoft.AspNetCore.Identity;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
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
            var result = await _signInManager.PasswordSignInAsync(userForLogin.Email, userForLogin.Password, false, false);

            if (!result.Succeeded)
            {
                // TODO: custom exception and handle it in custom exception middleware
                throw new Exception("Failed to sign in!");
            }

            var user = _userManager.Users.SingleOrDefault(r => r.Email == userForLogin.Email);

            return _jwtFactory.Generate(userForLogin.Email, user).ToString();
        }

        public async Task Register(RegisterDto userForRegistration)
        {
            // TODO: add fluent validation
            var user = new User
            {
                UserName = userForRegistration.Email,
                Email = userForRegistration.Email
            };

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded)
                throw new Exception("failed to login!");
                //var errors = result.Errors.Select(x => x.Description);

        }
    }
}
