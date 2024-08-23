using Microsoft.EntityFrameworkCore;
using Notebook.Server.Domain;
using Notebook.Server.Enum;
using Notebook.Server.Helper;

namespace Notebook.Server.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Note> Notebooks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RestoreUser> RestoreUserAccount { get; set; }
        public DbSet<Role> Roles { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(option =>
            {
                option.HasKey(f => f.Email);
                option.HasMany(f => f.Notes)
                      .WithOne(f => f.User)
                      .HasForeignKey(f => f.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                option.HasMany(f => f.Roles)
                      .WithMany(f => f.Users)
                      .UsingEntity<UserRole>(
                          j => j.HasOne(ur => ur.Role)
                                .WithMany()
                                .HasForeignKey(ur => ur.RolesId),
                          j => j.HasOne(ur => ur.User)
                                .WithMany()
                                .HasForeignKey(ur => ur.UsersId),
                          je =>
                          {
                              je.HasKey(ur => new { ur.RolesId, ur.UsersId });
                          });
            });

            modelBuilder.Entity<Note>(option =>
            {
                option.HasKey(f => f.Id);
            });

            modelBuilder.Entity<Role>(option =>
            {
                option.HasKey(f => f.RoleName);
            });

            // Начальные данные для ролей
            var roles = new[]
            {
                new Role
                {
                    RoleName = RoleName.Admin
                },
                new Role
                {
                    RoleName = RoleName.User
                }
            };

            // Создание начального пользователя
            var user = InitializeUsers.CreateAdmin("admin@notebook.com", "Admin1!");

            var users = new[]
            {
                user
            };

            // Связь пользователя и роли через UserRole
            var roleUser = new[]
            {
            new UserRole
                {
                    RolesId = RoleName.Admin,
                    UsersId = "admin@notebook.com"
                }
            };

            // Добавление данных в базы
            modelBuilder.Entity<Role>().HasData(roles);
            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<UserRole>().HasData(roleUser);

        }
    }
}
