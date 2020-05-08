using Microsoft.AspNetCore.Mvc;
using Sindikat.Identity.API.ActionFilters;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Common.Enums;
using System.Threading.Tasks;

namespace Sindikat.Identity.API.Controllers
{
    [AuthorizeRoles(Roles.Admin)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;
        private readonly IClaimValidatorService _claimValidatorService;

        public ClaimController(IClaimService claimService, IClaimValidatorService claimValidatorService)
        {
            _claimService = claimService;
            _claimValidatorService = claimValidatorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var claims = await _claimService.GetAll();

            return Ok(claims);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var claim = await _claimService.GetOne(id);

            return Ok(claim);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClaimForSaveDto claimForSave)
        {
            await _claimValidatorService.ValidateBeforeSave(claimForSave);

            await _claimService.Create(claimForSave);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClaimForSaveDto claimForSave)
        {
            await _claimValidatorService.ValidateBeforeUpdate(id, claimForSave);

            await _claimService.Update(id, claimForSave);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _claimValidatorService.ValidateBeforeDelete(id);

            await _claimService.Delete(id);

            return Ok();
        }

    }
}
