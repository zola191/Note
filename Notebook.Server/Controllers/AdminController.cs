using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Dto;
using Notebook.Server.Services;
using Notebook.Server.Validations;

namespace Notebook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly INoteService noteService;
        private readonly IUserService userService;

        public AdminController(INoteService noteService, IUserService userService)
        {
            this.noteService = noteService;
            this.userService = userService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var email = userService.GetUserEmail(Request);
            var notebooks = await noteService.GetAllAsync(email);
            return Ok(notebooks);
        }
    }
}
