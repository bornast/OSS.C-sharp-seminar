using Sindikat.Identity.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
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
