using Sindikat.Identity.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(LoginDto userForLogin);
        Task Register(RegisterDto userForRegistration);
    }
}
