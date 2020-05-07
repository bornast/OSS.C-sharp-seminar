using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Application.Dtos
{
    public class LoginSuccessDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
