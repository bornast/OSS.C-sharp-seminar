using Sindikat.Identity.Application.Dtos;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IUserValidatorService
    {
        Task ValidateBeforeUpdate(string userId, UserForUpdateDto userForUpdate);
        Task ValidateBeforeDelete(string userId);
    }
}
