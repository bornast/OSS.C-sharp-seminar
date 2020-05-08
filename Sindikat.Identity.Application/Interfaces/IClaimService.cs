using Sindikat.Identity.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IClaimService
    {
        Task<IEnumerable<ClaimDto>> GetAll();
        Task<ClaimDto> GetOne(int id);
        Task Create(ClaimForSaveDto claimForSave);
        Task Update(int claimId, ClaimForSaveDto claimForSave);
        Task Delete(int id);
    }
}
