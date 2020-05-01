using Sindikat.Identity.Domain.Entities;

namespace Sindikat.Identity.Persistence.Repository
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IdentityDbContext dbContext) : base(dbContext)
        {
        }
    }
}
