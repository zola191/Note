using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Dto;
using Notebook.Server.Services;

namespace Notebook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly INoteService noteService;
        private readonly IAdminService adminService;
        public AdminController(INoteService noteService, IAdminService adminService)
        {
            this.noteService = noteService;
            this.adminService = adminService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await adminService.GetAllUsers();
                return Ok(users);
            }
            //не знаю какая может быть ошибка из-за чего вопрос стоит ли такое обрабатывать ?
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Current([FromRoute] string email)
        {
            try
            {
                var currentUser = await adminService.GetCurrentUser(email);
                return Ok(currentUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await adminService.GetRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModelRequest userModel)
        {
            try
            {
                var result = await adminService.UpdateUser(userModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
