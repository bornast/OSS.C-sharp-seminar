using FluentValidation;
using Sindikat.Identity.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Application.Validators
{    
    public class ClaimForSaveDtoValidator : AbstractValidator<ClaimForSaveDto>
    {
        public ClaimForSaveDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
