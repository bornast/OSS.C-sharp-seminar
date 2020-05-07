using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Common.Exceptions;
using System.Threading.Tasks;

namespace Sindikat.Identity.API.Controllers
{
    // TODO: pass role enums
    [Authorize(Roles = "Admin")]
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
            await _claimValidatorService.ValidateForSave(claimForSave);

            await _claimService.Create(claimForSave);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClaimForSaveDto claimForSave)
        {
            await _claimValidatorService.ValidateForUpdate(id, claimForSave);

            await _claimService.Update(id, claimForSave);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _claimValidatorService.ValidateForDelete(id);

            await _claimService.Delete(id);

            return Ok();
        }

    }
}
