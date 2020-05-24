using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Application.Validators;
using Sindikat.Identity.Common.Enums;
using Sindikat.Identity.Common.Exceptions;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class UserValidatorService : BaseValidatorService, IUserValidatorService
    {
        private readonly IBaseRepository<User> _repo;

        public UserValidatorService(IBaseRepository<User> repo)
        {
            _repo = repo;
        }
        public async Task ValidateBeforeUpdate(string userId, UserForUpdateDto userForUpdate)
        {
            var validator = new UserForUpdateDtoValidator();

            CheckValidationResults(validator.Validate(userForUpdate));

            var user = await _repo.FindAsync(userId);

            if (user == null)
                throw new NotFoundException();

            var roles = user.UserRoles.Select(x => x.Role);

            var userAdminRole = roles.FirstOrDefault(x => x.Name == Enum.GetName(typeof(Roles), Roles.Admin));

            if (userAdminRole != null && !userForUpdate.RoleIds.Contains(userAdminRole.Id))
                ThrowValidationError("Role", $"Admin role cannot be removed!");

            if (userForUpdate.Claims.Any(x => String.IsNullOrEmpty(x.ClaimValue)))
                ThrowValidationError("Claim", $"Every selected claim must have a value!");
        }

        public async Task ValidateBeforeDelete(string userId)
        {
            var user = await _repo.FindAsync(userId);

            if (user == null)
                throw new NotFoundException();

            var roles = user.UserRoles.Select(x => x.Role);

            if (roles.Any(x => x.Name == Enum.GetName(typeof(Roles), Roles.Admin)))
                ThrowValidationError("User", $"User with role {Enum.GetName(typeof(Roles), Roles.Admin)} cannot be deleted!");
        }
    }
}
