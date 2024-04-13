using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Dto;
using Notebook.Server.Services;

namespace Notebook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AccountRequest accountRequest)
        {
            var response = await accountService.CreateAsync(accountRequest);
            if (response != null)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var existingAccount = await accountService.GetByEmail(loginRequest.Email);
            if (existingAccount == null)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}
