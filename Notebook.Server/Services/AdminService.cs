using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Data;
using Notebook.Server.Domain;
using Notebook.Server.Dto;
using Notebook.Server.Enum;

namespace Notebook.Server.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public AdminService(IMapper mapper, ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task DeleteUser(string email)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (existingUser != null)
            {
                dbContext.Users.Remove(existingUser);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            var existingUsers = await dbContext.Users.Include(f => f.Roles).Include(f => f.Notes).ToListAsync();
            var userModels = mapper.Map<List<UserModel>>(existingUsers);
            return userModels;
        }

        public async Task<UserModel> GetCurrentUser(string email)
        {
            var existingUser = await dbContext.Users.Include(f => f.Roles).Include(f => f.Notes).FirstOrDefaultAsync(f => f.Email == email);
            var userModel = mapper.Map<UserModel>(existingUser);
            return userModel;
        }

        public async Task<List<RoleModel>> GetRoles()
        {
            var existingRoles = await dbContext.Roles.ToListAsync();
            var result = mapper.Map<List<RoleModel>>(existingRoles);
            return result;
        }

        public async Task<UserModel> UpdateUser(UserModelRequest user)
        {
            var existingUser = await dbContext.Users.Include(f => f.Roles).FirstOrDefaultAsync(f => f.Email == user.Email);

            //var roles = await dbContext.Roles.Where(f => user.RoleModels.Any(g =>
            //{
            //    var x = (RoleName)System.Enum.Parse(typeof(RoleName), g.RoleName);
            //    if (x == f.RoleName)
            //    {
            //        return true;
            //    }
            //    return false;
            //}));

            //var roles = new List<Role>();

            //foreach (var roleModel in user.RoleModels)
            //{
            //    var roleName = roleModel.RoleName;
            //    var x = (RoleName)System.Enum.Parse(typeof(RoleName), roleModel.RoleName);

            //    var role = new Role()
            //    {
            //        RoleName = (RoleName)System.Enum.Parse(typeof(RoleName), roleModel.RoleName)
            //    };
            //    roles.Add(role);
            //}

            var roles = user.RoleModels.Select(f => (RoleName)System.Enum.Parse(typeof(RoleName), f.RoleName));
            var rolesFromDb = await dbContext.Roles.Where(f=>roles.Any(g=>g==f.RoleName)).ToListAsync();

            if (existingUser != null)
            {
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                //existingUser.Roles = roles;
                existingUser.Roles = rolesFromDb;
                //existingUser.Roles = mapper.Map<List<Role>>(user.RoleModels);
                //mapper.Map<List<Role>>(user.RoleModels);

                dbContext.Update(existingUser);

                await dbContext.SaveChangesAsync();

                var result = mapper.Map<UserModel>(existingUser);
                return result;
            }
            return null;
        }
    }
}
