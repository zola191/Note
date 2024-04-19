using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Data;
using Notebook.Server.Domain;
using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        public AccountService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<AccountModel> CreateAsync(AccountRequest request)
        {
            var account = mapper.Map<Account>(request);

            if (account.User == null)
            {
                var user = account.User = new User()
                {
                    Email = account.Email,
                    Account = account,
                    AccountId = account.Email,
                };
                await dbContext.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }

            //await dbContext.AddAsync(account);
            //await dbContext.SaveChangesAsync();

            var response = mapper.Map<AccountModel>(account);
            //var userModel = await userService.CreateAsync(accountModel);

            return response;
        }

        public async Task<AccountModel> FindByEmail(string email)
        {
            var existingAccount = await dbContext.Accounts.FirstOrDefaultAsync(f => f.Email == email);

            var response = mapper.Map<AccountModel>(existingAccount);
            return response;
        }

        public string GetUserEmail(HttpRequest request)
        {
            //выполнить реализацию метода;
            return "admin@notebook";
        }
    }
}
