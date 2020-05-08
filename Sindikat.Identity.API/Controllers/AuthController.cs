using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Sindikat.Identity.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAuthValidatorService _authValidatorService;
        private readonly IDistributedCache _distributedCache;

        public AuthController(IAuthService authService, IAuthValidatorService authValidatorService, IDistributedCache distributedCache)
        {
            _authService = authService;
            _authValidatorService = authValidatorService;
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto userForLogin)
        {
            _authValidatorService.ValidateBeforeLogin(userForLogin);

            var token = await _authService.Login(userForLogin);

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto userForRegistration)
        {
            await _authValidatorService.ValidateBeforeRegistration(userForRegistration);

            await _authService.Register(userForRegistration);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(TokenForRefreshDto tokenForRefresh)
        {
            await _authValidatorService.ValidateBeforeTokenRefresh(tokenForRefresh);

            var token = await _authService.RefreshToken(tokenForRefresh);

            return Ok(token);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            var token = HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Authorization")
                .Value.FirstOrDefault().Substring("Bearer".Length + 1);

            await _authService.SignOut(token);

            return Ok();
        }
    }
}