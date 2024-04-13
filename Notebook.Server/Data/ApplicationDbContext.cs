using Microsoft.EntityFrameworkCore;
using Notebook.Server.Domain;

namespace Notebook.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Note> Notebooks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
