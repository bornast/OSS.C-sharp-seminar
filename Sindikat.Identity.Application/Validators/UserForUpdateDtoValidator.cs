using FluentValidation;
using Sindikat.Identity.Application.Dtos;

namespace Sindikat.Identity.Application.Validators
{
    public class UserForUpdateDtoValidator : AbstractValidator<UserForUpdateDto>
    {
        public UserForUpdateDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.RoleIds).NotEmpty().WithMessage("User must have at least 1 role!");
        }
    }
}
