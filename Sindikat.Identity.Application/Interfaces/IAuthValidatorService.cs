using Sindikat.Identity.Application.Dtos;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IAuthValidatorService
    {
        void ValidateForLogin(LoginDto userForLogin);
        Task ValidateForRegistration(RegisterDto userForRegistration);
    }
}
