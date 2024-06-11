using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Authentication;
using Notebook.Server.Dto;
using Notebook.Server.Services;

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
        public async Task<IActionResult> Create([FromBody] AccountRequest accountRequest)
        {
            try
            {
                var existingAccount = await accountService.FindByEmail(accountRequest.Email);
                var response = await accountService.CreateAsync(accountRequest);
                return Ok(response);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountRequest request)
        {
            try
            {
                var existingAccount = await accountService.CheckLogin(request);
                return Ok(existingAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
