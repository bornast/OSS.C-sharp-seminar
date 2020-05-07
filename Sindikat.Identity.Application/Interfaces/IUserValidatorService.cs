using Sindikat.Identity.Application.Dtos;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IUserValidatorService
    {
        Task ValidateForUpdate(string userId, UserForUpdateDto userForUpdate);
        Task ValidateForDelete(string userId);
    }
}
