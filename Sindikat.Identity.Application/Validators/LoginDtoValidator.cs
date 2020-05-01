using FluentValidation;
using Sindikat.Identity.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(3);
        }
    }    
}
