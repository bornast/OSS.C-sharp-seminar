using FluentValidation;
using Sindikat.Identity.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(3);
        }
    }
}
