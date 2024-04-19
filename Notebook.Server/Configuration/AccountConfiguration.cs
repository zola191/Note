using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notebook.Server.Domain;

namespace Notebook.Server.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(f => f.Email).IsRequired();
            builder.HasKey(f=>f.Email);
            builder.Property(f => f.Password).IsRequired();

            builder.HasOne(e => e.User)
            .WithOne(e => e.Account)
            .HasForeignKey<User>(e => e.AccountId);
        }
    }
}
