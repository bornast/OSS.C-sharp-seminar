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
        private readonly IAuthValidatorService _authValidatorService;

        public AuthController(IAuthService authService, IAuthValidatorService authValidatorService)
        {            
            _authService = authService;
            _authValidatorService = authValidatorService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto userForLogin)
        {
            _authValidatorService.ValidateForLogin(userForLogin);

            var token = await _authService.Login(userForLogin);

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto userForRegistration)
        {
            await _authValidatorService.ValidateForRegistration(userForRegistration);

            await _authService.Register(userForRegistration);

            return Ok();
        }             

    }
}