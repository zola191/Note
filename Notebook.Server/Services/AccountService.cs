using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Data;
using Notebook.Server.Domain;
using Notebook.Server.Dto;
using System.IdentityModel.Tokens.Jwt;

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
            var user = new User()
            {
                Email = request.Email,
                AccountId = request.Email,
            };

            var account = mapper.Map<Account>(request);
            user.Account = account;
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            var response = mapper.Map<AccountModel>(account);

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
            var handler = new JwtSecurityTokenHandler();
            var jwt = request.Headers.Authorization.ToString().Split().Last();
            var token = handler.ReadJwtToken(jwt);
            var email = token.Claims.Select(claim => claim.Value).First();

            return email;
        }
    }
}
