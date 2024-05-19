using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Authentication;
using Notebook.Server.Dto;
using Notebook.Server.Services;

namespace Notebook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly IAccountService accountService;
        private readonly IJwtProvider jwtProvider;
        public AccountController(IAccountService accountService, IJwtProvider jwtProvider)
        {
            this.accountService = accountService;
            this.jwtProvider = jwtProvider;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AccountRequest accountRequest)
        {
            var existingAccount = await accountService.FindByEmail(accountRequest.Email);

            if (existingAccount != null)
            {
                throw new Exception("Account is already exist");
            }

            // передать в cookie token
            // внести правки в модели

            // 1. Hash with salt password хранить в БД
            // 2. Выполнить проверку подтверждения пароля во фронте перед отправкой в бэк
            // 3. Выполнить процедуру восстановления пароля (ссылка для генерации нового пароля)
            // 4. Регистрация в Azure
            // 5. ChatGpt

            var response = await accountService.CreateAsync(accountRequest);

            return Ok(response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var existingAccount = await accountService.CheckLogin(request);

            if (existingAccount == null)
            {
                throw new Exception("Bad login or password");
            }

            var token = jwtProvider.Generate(existingAccount);
            existingAccount.Token = token;

            return Ok(existingAccount);
        }

        [HttpPost("restore")]
        public async Task<IActionResult> RestorePassword([FromBody] AccountRestoreRequest request)
        {
            var existingAccount = await accountService.RestorePassword(request);
            if (existingAccount == null)
            {
                throw new Exception("Bad login");
            }
            return Ok(existingAccount);
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromQuery] string token, [FromBody] AccountRequest request)
        {
            var existingAccount = await accountService.FindByEmail(request.Email);
            var isExpiredToken = accountService.CheckToken(token);
            if (isExpiredToken)
            {
                return BadRequest();
            }
            await accountService.ChangePassword(existingAccount,request.Password);
            return Ok();
        }
    }
}
