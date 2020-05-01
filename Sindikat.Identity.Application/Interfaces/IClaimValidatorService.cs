using Sindikat.Identity.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IClaimValidatorService
    {
        Task ValidateForSave(ClaimForSaveDto claimForSave);
        Task ValidateForUpdate(int claimId, ClaimForSaveDto claimForSave);
        Task ValidateForDelete(int claimId);
    }
}
