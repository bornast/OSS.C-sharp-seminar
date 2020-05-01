using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base()
        {
        }
        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}
