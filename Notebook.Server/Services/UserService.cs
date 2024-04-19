using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Data;
using Notebook.Server.Domain;
using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public UserService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<UserModel> FindByEmail(string email)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(f => f.Email == email);

            if (existingUser == null)
            {
                return null;
            }

            var response = mapper.Map<UserModel>(existingUser);
            return response;
        }

        public async Task<UserModel> CreateAsync(AccountModel accountModel)
        {
            var newUser = new User()
            {
                Email = accountModel.Email,
                Account = mapper.Map<Account>(accountModel)
            };
            await dbContext.AddAsync(newUser);
            await dbContext.SaveChangesAsync();

            var respone = mapper.Map<UserModel>(newUser);
            return respone;
        }

        public async Task<UserModel> DeleteAsync(AccountModel accountModel)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(f => f.Email == accountModel.Email);
            dbContext.Users.Remove(existingUser);
            await dbContext.SaveChangesAsync();
            var response = mapper.Map<UserModel>(existingUser);
            return response;
        }
    }
}
