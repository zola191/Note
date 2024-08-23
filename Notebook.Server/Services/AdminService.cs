using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Data;
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

        public async Task DeleteNoteAsync(int id)
        {
            var existingNote = await dbContext.Notebooks.FirstOrDefaultAsync(f => f.Id == id);
            if (existingNote != null)
            {
                dbContext.Remove(existingNote);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(string email)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (existingUser != null)
            {
                dbContext.Users.Remove(existingUser);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var existingUsers = await dbContext.Users.Include(f => f.Roles).Include(f => f.Notes).ToListAsync();
            var userModels = mapper.Map<List<UserModel>>(existingUsers);
            return userModels;
        }

        public async Task<NoteModel> GetCurrenNoteAsync(int id)
        {
            var existingNote = await dbContext.Notebooks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingNote != null)
            {
                var noteModel = mapper.Map<NoteModel>(existingNote);
                return noteModel;
            }
            return null;
        }

        public async Task<UserModel> GetCurrentUserAsync(string email)
        {
            var existingUser = await dbContext.Users.Include(f => f.Roles).Include(f => f.Notes).FirstOrDefaultAsync(f => f.Email == email);
            var userModel = mapper.Map<UserModel>(existingUser);
            return userModel;
        }

        public async Task<List<RoleModel>> GetRolesAsync()
        {
            var existingRoles = await dbContext.Roles.ToListAsync();
            var result = mapper.Map<List<RoleModel>>(existingRoles);
            return result;
        }

        public async Task<bool> UpdateNoteAsync(int id, NoteRequest request)
        {
            var existingNote = await dbContext.Notebooks.FirstOrDefaultAsync(f => f.Id == id);
            if (existingNote != null)
            {
                existingNote.FirstName = request.FirstName;
                existingNote.MiddleName = request.FirstName;
                existingNote.LastName = request.LastName;
                existingNote.PhoneNumber = request.PhoneNumber;
                existingNote.Country = request.Country;
                existingNote.BirthDay = request.BirthDay;
                existingNote.Organization = request.Organization;
                existingNote.Position = request.Position;
                existingNote.Other = request.Other;

                dbContext.Update(existingNote);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<UserModel> UpdateUserAsync(UserModelRequest user)
        {
            var existingUser = await dbContext.Users.Include(f => f.Roles).Include(f=>f.Notes).FirstOrDefaultAsync(f => f.Email == user.Email);

            var roles = user.RoleModels.Select(f => (RoleName)System.Enum.Parse(typeof(RoleName), f.RoleName));
            var rolesFromDb = await dbContext.Roles.Where(f => roles.Any(g => g == f.RoleName)).ToListAsync();

            if (existingUser != null)
            {
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Roles = rolesFromDb;

                dbContext.Update(existingUser);

                await dbContext.SaveChangesAsync();

                var result = mapper.Map<UserModel>(existingUser);
                return result;
            }
            return null;
        }
    }
}
