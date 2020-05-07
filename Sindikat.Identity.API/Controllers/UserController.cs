using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sindikat.Identity.Domain.Models.User;
using Sindikat.Identity.Persistence.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Common.Exceptions;
using Sindikat.Identity.Application.Dtos;

namespace Sindikat.Identity.API.Controllers
{
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
            await _userValidatorService.ValidateForUpdate(id, userForUpdate);

            await _userService.Update(id, userForUpdate);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userValidatorService.ValidateForDelete(id);

            await _userService.Delete(id);

            return Ok();
        }

    }
}