/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Authentication;
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
        private readonly IJwtProvider jwtProvider;
        private readonly IEmailService emailService;
        private readonly IPasswordHasher passwordHasher;
        public AccountService(ApplicationDbContext dbContext, IMapper mapper, IJwtProvider jwtProvider, IEmailService emailService, IPasswordHasher passwordHasher)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.jwtProvider = jwtProvider;
            this.emailService = emailService;
            this.passwordHasher = passwordHasher;
        }

        public async Task<AccountModel> CreateAsync(CreateAccountRequest request)
        {
            var user = new User()
            {
                Email = request.Email,
                AccountId = request.Email,
            };

            var account = mapper.Map<Account>(request);
            var salt = passwordHasher.CreateSalt();

            account.PasswordHash = passwordHasher.HashPassword(request.Password, salt);
            account.Salt = salt;

            user.Account = account;

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var response = mapper.Map<AccountModel>(account);

            return response;
        }

        public async Task<AccountModel> CreateWithGoogleAsync(string userEmail)
        {
            var user = new User()
            {
                Email = userEmail,
                AccountId = userEmail
            };

            var account = new Account()
            {
                Email = user.Email,
                User = user,
            };

            user.Account = account;

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var response = mapper.Map<AccountModel>(new Account()
            {
                Email = user.Email,
            });

            return response;
        }

        public async Task<AccountModel> FindByEmail(string email)
        {
            var existingAccount = await dbContext.Accounts.FirstOrDefaultAsync(f => f.Email == email);
            if (existingAccount != null)
            {
                throw new Exception("Account is already exist");
            }
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

        public async Task<RestoreAccountModel> RestorePassword(AccountRestoreRequest request)
        {
            var existingAccount = await dbContext.Accounts.FirstOrDefaultAsync(f => f.Email == request.Email);

            if (existingAccount == null)
            {
                throw new Exception("Account does not exist");
            }

            var token = jwtProvider.GenerateRestore(existingAccount);

            var restoreAccount = new RestoreAccount()
            {
                Account = existingAccount,
                Token = token,
                Validity = DateTime.Now.AddHours(12),
            };

            await dbContext.AddAsync(restoreAccount);
            await dbContext.SaveChangesAsync();

            var email = new EmailModel()
            {
                To = "stefan.bergstrom76@ethereal.email",
                Subject = $"Ссылка на восстановление доступа",
                Body = $"http://localhost:4200/account/restore/{token}"
            };

            emailService.SendEmail(email);

            var response = mapper.Map<RestoreAccountModel>(restoreAccount);
            return response;
        }

        public async Task<AccountModel> CheckLogin(LoginAccountRequest request)
        {
            var existingAccount = await dbContext.Accounts.FirstOrDefaultAsync(f => f.Email == request.Email);

            if (existingAccount == null)
            {
                throw new Exception("Bad login or password");
            }

            var passwordHash = passwordHasher.HashPassword(request.Password, existingAccount.Salt);

            if (existingAccount.PasswordHash != passwordHash)
            {
                throw new Exception("Bad login or password");
            }

            var result = mapper.Map<AccountModel>(existingAccount);
            result.Token = jwtProvider.Generate(result);

            return result;
        }

        //Проверить!
        public bool IsExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;
            var now = DateTime.Now.ToUniversalTime();
            var isExpired = tokenDate >= now;

            if (isExpired)
            {
                return false;
            }

            return true;
        }

        public async Task ChangePasswordAsync(ChangePasswordModel model)
        {
            var tokenIsExpired = IsExpired(model.Token);

            if (!tokenIsExpired)
            {
                throw new Exception("Time to restore account expired");
            }

            var existingAccount = await FindByToken(model.Token);

            if (existingAccount == null)
            {
                throw new Exception("Wrong Url to restore password");
            }

            existingAccount.PasswordHash = passwordHasher.HashPassword(model.Password,existingAccount.Salt);

            dbContext.Users.Update(existingAccount);
            await dbContext.SaveChangesAsync();
        }

        public async Task<User> FindByToken(string token)
        {
            var existingRestoreAccount = await dbContext.RestoreAccount
                .Include(f => f.Account)
                .FirstOrDefaultAsync(f => f.Token == token);

            var account = await dbContext.Users
                .FirstOrDefaultAsync(f => f.Email == existingRestoreAccount.Account.Email);

            return account;
        }
    }
}
*/