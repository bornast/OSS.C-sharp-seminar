using FluentValidation;
using Sindikat.Identity.Application.Dtos;

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
