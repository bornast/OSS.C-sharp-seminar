using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.API.ActionFilters;
using Sindikat.Identity.Common.Enums;

namespace Sindikat.Identity.API.Controllers
{
    [AuthorizeRoles(Roles.Admin)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserValidatorService _userValidatorService;

        public UserController(IUserService userService, IUserValidatorService userValidatorService)
        {
            _userService = userService;
            _userValidatorService = userValidatorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var user = await _userService.GetOne(id);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UserForUpdateDto userForUpdate)
        {
            await _userValidatorService.ValidateBeforeUpdate(id, userForUpdate);

            await _userService.Update(id, userForUpdate);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userValidatorService.ValidateBeforeDelete(id);

            await _userService.Delete(id);

            return Ok();
        }

    }
}