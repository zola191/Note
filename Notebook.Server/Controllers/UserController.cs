using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Authentication;
using Notebook.Server.Dto;
using Notebook.Server.Exceptions;
using Notebook.Server.Services;
using Notebook.Server.Validators;

// передать в cookie token
// внести правки в модели

// 1. Hash with salt password хранить в БД
// 2. Выполнить проверку подтверждения пароля во фронте перед отправкой в бэк
// 3. Выполнить процедуру восстановления пароля (ссылка для генерации нового пароля)
// 4. Регистрация в Azure
// 5. ChatGpt


// Добавить импорт Note для своего пользователя из файлика
// Добавить кнопку импорта на фронт
// При нажатии дефолт браузер окно выбрать файлик
// Выбрал файл нажал ок
// фронт отправляет в бэк файл в исходном виде на спец метод контроллера importController
// бэк получил файл, распарсить файл и создать note, сохранить в БД и вернут на фронт результат
// фронт ok result и обновляю страницу
//* toaster сделать snackbar результата запроса Ok или Bad


namespace Notebook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IJwtProvider jwtProvider;
        private readonly IUserService userService;
        public UserController(IJwtProvider jwtProvider, IUserService userService)
        {
            this.jwtProvider = jwtProvider;
            this.userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var validator = new UserRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var response = await userService.CreateAsync(request);
                return Ok(response);
            }

            catch (UserAlreadyExistException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAccountRequest request)
        {
            try
            {
                var existingUser = await userService.CheckUser(request);
                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("loginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] LoginWithGoogleRequest request)
        {
            try
            {
                var existingUser = await userService.CheckGoogleUser(request);
                return Ok(existingUser);
            }
            //неизвестная ошибка?????
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("restore")]
        public async Task<IActionResult> RestorePassword([FromBody] RestoreUserModel request)
        {
            try
            {
                var existingAccount = await userService.RestorePassword(request);
                return Ok(existingAccount);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordModel request)
        {
            try
            {
                await userService.ChangePasswordAsync(request);
                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UserInfo")]
        public async Task<IActionResult> UserInfo(UserInfoModel model)
        {
            try
            {
                var userModel = await userService.FindByEmail(model.Email);
                return Ok(new UserInfoModel()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                });
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
