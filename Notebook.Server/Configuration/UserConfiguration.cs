using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notebook.Server.Domain;

namespace Notebook.Server.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(f => f.Email);
            builder.HasMany(f=>f.Notes)
                   .WithOne(f=>f.User)
                   .HasForeignKey(f=>f.UserId);

            builder.HasOne(e => e.Account)
                   .WithOne(e => e.User)
                   .HasForeignKey<Account>(e => e.UserId);
        }
    }
}
