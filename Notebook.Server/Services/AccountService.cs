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

            var existingAccount = await dbContext.Accounts.FirstOrDefaultAsync(f => f.Email == account.Email);

            if (existingAccount != null)
            {
                return null;
            }

            await dbContext.AddAsync(account);
            await dbContext.SaveChangesAsync();

            var response = mapper.Map<AccountModel>(account);
            return response;
        }

        public async Task<AccountModel> GetByEmail(string email)
        {
            var existingAccount = await dbContext.Accounts.FirstOrDefaultAsync(f => f.Email == email);

            var response = mapper.Map<AccountModel>(existingAccount);
            return response;
        }
    }
}
