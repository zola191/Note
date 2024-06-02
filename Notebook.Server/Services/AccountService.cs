using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Authentication;
using Notebook.Server.Data;
using Notebook.Server.Domain;
using Notebook.Server.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Notebook.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IJwtProvider jwtProvider;
        private readonly IEmailService emailService;
        public AccountService(ApplicationDbContext dbContext, IMapper mapper, IJwtProvider jwtProvider, IEmailService emailService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.jwtProvider = jwtProvider;
            this.emailService = emailService;
        }

        public async Task<AccountModel> CreateAsync(AccountRequest request)
        {
            var user = new User()
            {
                Email = request.Email,
                AccountId = request.Email,
            };

            var account = mapper.Map<Account>(request);

            account.Password = HashPassword(account.Password, out byte[] salt);

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

        public async Task<RestoreAccountModel> RestorePassword(AccountRestoreRequest request)
        {
            var existingAccount = await dbContext.Accounts.FirstOrDefaultAsync(f => f.Email == request.Email);
            if (existingAccount == null)
            {
                return null;
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
                To = "salma.kiehn5@ethereal.email",
                Subject = $"Ссылка на восстановление доступа",
                Body = $"http://localhost:4200/account/restore/{token}"
            };
            emailService.SendEmail(email);

            var response = mapper.Map<RestoreAccountModel>(restoreAccount);
            return response;
        }

        private string HashPassword(string passowrd, out byte[] salt)
        {
            const int keySize = 64;
            const int interations = 400000;
            var hashAlgoritm = HashAlgorithmName.SHA256;

            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(passowrd),
                salt, interations, hashAlgoritm, keySize);

            return Convert.ToHexString(hash);
        }

        private bool VerifyPassword(string password, string hash, byte[] salt)
        {
            const int keySize = 64;
            const int interations = 400000;
            var hashAlgoritm = HashAlgorithmName.SHA256;

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, interations, hashAlgoritm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

        public Task<AccountModel> CheckLogin(LoginRequest request)
        {
            var existingAccount = FindByEmail(request.Email);
            if (existingAccount == null)
            {
                return null;
            }
            var hash = HashPassword(request.Password, out byte[] salt);
            var checkPassword = VerifyPassword(request.Password, hash, salt);
            if (!checkPassword)
            {
                return null;
            }
            return existingAccount;
        }


        public bool CheckToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;
            var now = DateTime.Now.ToUniversalTime();
            var valid = tokenDate >= now;

            if (!valid)
            {
                return false;
            }

            return true;
        }

        public async Task ChangePasswordAsync(Account account, string newPassword)
        {
            ////выполнить копию объекта без сохранения ссылки
            //var existringAccount = await dbContext.Accounts.FirstOrDefaultAsync(f => f.Email == account.Email);
            account.Password = newPassword;
            dbContext.Accounts.Update(account);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Account> FindByToken(string token)
        {
            var existingRestoreAccount = await dbContext.RestoreAccount.Include(f=>f.Account).FirstOrDefaultAsync(f => f.Token == token);
            var account = await dbContext.Accounts.FirstOrDefaultAsync(f => f.Email == existingRestoreAccount.Account.Email);
            return account;
        }
    }
}
