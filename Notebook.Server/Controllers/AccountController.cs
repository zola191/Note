using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Notebook.Server.Authentication;
using Notebook.Server.Dto;
using Notebook.Server.Services;
using Notebook.Server.Validators;

// передать в cookie token
// внести правки в модели

// 1. Hash with salt password хранить в БД
// 2. Выполнить проверку подтверждения пароля во фронте перед отправкой в бэк
// 3. Выполнить процедуру восстановления пароля (ссылка для генерации нового пароля)
// 4. Регистрация в Azure
// 5. ChatGpt

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
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
        {
            var validator = new AccountRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var existingAccount = await accountService.FindByEmail(request.Email);
                var response = await accountService.CreateAsync(request);
                return Ok(response);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAccountRequest request)
        {
            try
            {
                var existingAccount = await accountService.CheckLogin(request);
                //if (Request.Cookies.TryGetValue("token", out string err))
                //{

                //}
                //var options = new CookieOptions();
                //options.Secure = true;
                ////options.Expires = DateTime.Now.AddDays(1);
                //Response.Cookies.Append("token", existingAccount.Token);

                var cookieOptions = new CookieOptions();
                cookieOptions.Secure = true;

                HttpContext.Response.Cookies.Append("token", existingAccount.Token, cookieOptions);

                return Ok(existingAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("loginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
        {
            return Ok();
        }

        [HttpPost("restore")]
        public async Task<IActionResult> RestorePassword([FromBody] AccountRestoreRequest request)
        {
            try
            {
                var existingAccount = await accountService.RestorePassword(request);
                return Ok(existingAccount);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel request)
        {
            try
            {
                await accountService.ChangePasswordAsync(request);
                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
