using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sindikat.Identity.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Sindikat.Identity.Application.Dtos;

namespace Sindikat.Identity.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {        
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {            
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var token = await _authService.Login(model);

            return new JsonResult(token);
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDto model)
        {
            await _authService.Register(model);

            return Ok();
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            

            return Ok();
        }

        [Authorize(Roles = "NotAdmin")]
        [HttpGet]
        public async Task<IActionResult> NotAdmin()
        {


            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Anyone()
        {


            return Ok();
        }

    }
}