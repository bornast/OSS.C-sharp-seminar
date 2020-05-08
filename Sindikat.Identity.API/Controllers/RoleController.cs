using Microsoft.AspNetCore.Mvc;
using Sindikat.Identity.API.ActionFilters;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Common.Enums;
using System.Threading.Tasks;

namespace Sindikat.Identity.API.Controllers
{
    [AuthorizeRoles(Roles.Admin)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAll();

            return Ok(roles);
        }
    }
}
