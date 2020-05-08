using Sindikat.Identity.Application.Dtos;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IClaimValidatorService
    {
        Task ValidateBeforeSave(ClaimForSaveDto claimForSave);
        Task ValidateBeforeUpdate(int claimId, ClaimForSaveDto claimForSave);
        Task ValidateBeforeDelete(int claimId);
    }
}
