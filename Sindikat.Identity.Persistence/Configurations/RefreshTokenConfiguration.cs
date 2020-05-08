using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindikat.Identity.Domain.Entities;

namespace Sindikat.Identity.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Token);
            builder.Property(x => x.Token).HasDefaultValueSql("NEWID()");
        }
    }
}
