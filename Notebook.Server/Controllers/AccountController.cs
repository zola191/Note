using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Notebook.Server.Dto;
using Notebook.Server.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notebook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly IAccountService accountService;
        private IConfiguration _config;

        public AccountController(IAccountService accountService, IConfiguration config)
        {
            this.accountService = accountService;
            _config = config;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AccountRequest accountRequest)
        {
            var existingAccount = await accountService.FindByEmail(accountRequest.Email);
            if (existingAccount != null)
            {
                throw new Exception("Account is already exist");
            }
            var response = await accountService.CreateAsync(accountRequest);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var existingAccount = await accountService.FindByEmail(loginRequest.Email);

            if (existingAccount == null)
            {
                throw new Exception("Bad login or password");
            }

            if (existingAccount.Password != loginRequest.Password)
            {
                throw new Exception("Bad password");
            }

            var token = Generate(existingAccount);
            existingAccount.Token = token;

            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            cookieOptions.Path = "/";
            
            //Не получилось сразу отправить в cookie 
            Response.Cookies.Append("tasty-cookies", token, cookieOptions);

            return Ok(existingAccount);
        }

        private string Generate(AccountModel account)
        {
            var secutiryKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(secutiryKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,account.Email),
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
