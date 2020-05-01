using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sindikat.Identity.Application.Dtos
{
    public class RegisterDto
    {        
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}
