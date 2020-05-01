using Sindikat.Identity.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Persistence
{
    class IdentityDbContextFactory : DesignTimeDbContextFactoryBase<IdentityDbContext>
    {
        protected override IdentityDbContext CreateNewInstance(DbContextOptions<IdentityDbContext> options)
        {
            return new IdentityDbContext(options);
        }
    }
}
