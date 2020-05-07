using Microsoft.AspNetCore.Identity;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Application.Validators;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class AuthValidatorService : BaseValidatorService, IAuthValidatorService
    {
        private readonly UserManager<User> _userManager;

        public AuthValidatorService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public void ValidateForLogin(LoginDto userForLogin)
        {
            var validator = new LoginDtoValidator();

            CheckValidationResults(validator.Validate(userForLogin));
        }

        public async Task ValidateForRegistration(RegisterDto userForRegistration)
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

    }
}
