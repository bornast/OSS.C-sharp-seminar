using Sindikat.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface IJwtFactory
    {
        object Generate(User user);
    }
}
