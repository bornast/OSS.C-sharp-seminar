using Microsoft.EntityFrameworkCore;
using Sindikat.Identity.Persistence.Infrastructure;

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
