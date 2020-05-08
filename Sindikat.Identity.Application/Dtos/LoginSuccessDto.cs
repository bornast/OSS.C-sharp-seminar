using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Application.Dtos
{
    public class TokenAndRefreshTokenPairDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
