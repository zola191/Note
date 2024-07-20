using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Authentication;
using Notebook.Server.Data;
using Notebook.Server.Domain;
using Notebook.Server.Dto;
using System.IdentityModel.Tokens.Jwt;

namespace Notebook.Server.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IPasswordHasher passwordHasher;
        private readonly IJwtProvider jwtProvider;
        private readonly IEmailService emailService;

        public UserService(ApplicationDbContext dbContext, IMapper mapper, IPasswordHasher passwordHasher, IEmailService emailService, IJwtProvider jwtProvider)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.passwordHasher = passwordHasher;
            this.emailService = emailService;
            this.jwtProvider = jwtProvider;
        }

        public async Task<UserModel> CreateAsync(CreateUserRequest request)
        {
            var salt = passwordHasher.CreateSalt();

            var newUser = new User()
            {
                Email = request.Email,
                Salt = salt,
                PasswordHash = passwordHasher.HashPassword(request.Password, salt)
            };

            await dbContext.AddAsync(newUser);
            await dbContext.SaveChangesAsync();

            var respone = mapper.Map<UserModel>(newUser);
            return respone;
        }

        public async Task<UserModel> DeleteAsync(UserModel model)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(f => f.Email == model.Email);
            dbContext.Users.Remove(existingUser);
            await dbContext.SaveChangesAsync();
            var response = mapper.Map<UserModel>(existingUser);
            return response;
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

        public async Task<GoogleUserModel> FindByGoogleEmail(string email)
        {
            var existingUser = await dbContext.ExternalGoogleUsers.Include(f=>f.Notes).FirstOrDefaultAsync(f => f.Email == email);

            if (existingUser == null)
            {
                return null;
            }

            var response = mapper.Map<GoogleUserModel>(existingUser);
            return response;
        }


        public async Task<UserModel> CheckUser(LoginAccountRequest request)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(f => f.Email == request.Email);

            if (existingUser == null)
            {
                throw new Exception("User does not exist");
            }

            var passwordHash = passwordHasher.HashPassword(request.Password, existingUser.Salt);

            if (existingUser.PasswordHash != passwordHash)
            {
                throw new Exception("Bad login or password");
            }

            var result = mapper.Map<UserModel>(existingUser);

            result.Token = jwtProvider.Generate(result);

            return result;
        }

        public async Task<GoogleUserModel> CheckGoogleUser(LoginWithGoogleRequest request)
        {
            //выглядит ужасно.....
            
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(request.Credential);
            var userEmail = decodedValue.Claims.ElementAt(4).Value;

            var existingUser = await FindByGoogleEmail(userEmail);

            if (existingUser == null)
            {
                var result = await CreateWithGoogleAsync(userEmail);
                var userModel = mapper.Map<UserModel>(result);

                result.Token = jwtProvider.Generate(userModel);
                return result;
            }
            existingUser.Token = jwtProvider.Generate(mapper.Map<UserModel>(existingUser));
            return existingUser;
        }

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

        public async Task<User> FindByToken(string token)
        {
            var existingRestoreAccount = await dbContext.RestoreUserAccount
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Token == token);

            var account = await dbContext.Users
                .FirstOrDefaultAsync(f => f.Email == existingRestoreAccount.User.Email);

            return account;
        }

        public async Task ChangePasswordAsync(ChangeUserPasswordModel model)
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

            existingAccount.PasswordHash = passwordHasher.HashPassword(model.Password, existingAccount.Salt);

            dbContext.Users.Update(existingAccount);
            await dbContext.SaveChangesAsync();
        }

        public async Task<GoogleUserModel> CreateWithGoogleAsync(string userEmail)
        {
            var existingUser = await dbContext.ExternalGoogleUsers.FirstOrDefaultAsync(f=>f.Email==userEmail);
            if (existingUser == null)
            {
                var user = new ExternalGoogleUser()
                {
                    Email = userEmail
                };

                dbContext.ExternalGoogleUsers.Add(user);
                await dbContext.SaveChangesAsync();
                return mapper.Map<GoogleUserModel>(user);
            }
            return mapper.Map<GoogleUserModel>(existingUser);
        }

        public async Task<UserModel> RestorePassword(RestoreUserModel model)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(f => f.Email == model.model.Email);

            if (existingUser == null)
            {
                throw new Exception("User does not exist");
            }

            var token = jwtProvider.GenerateRestore(existingUser);

            var restoreUser = new RestoreUserModel()
            {
                Token = token,
                Validity = DateTime.Now.AddHours(12),
            };

            await dbContext.AddAsync(restoreUser);
            await dbContext.SaveChangesAsync();

            var email = new EmailModel()
            {
                To = "stefan.bergstrom76@ethereal.email",
                Subject = $"Ссылка на восстановление доступа",
                Body = $"http://localhost:4200/account/restore/{token}"
            };

            emailService.SendEmail(email);

            var response = mapper.Map<UserModel>(restoreUser);
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
