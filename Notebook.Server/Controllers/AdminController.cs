using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Dto;
using Notebook.Server.Services;

namespace Notebook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await adminService.GetAllUsersAsync();
                return Ok(users);
            }
            //не знаю какая может быть ошибка из-за чего вопрос стоит ли такое обрабатывать ?
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> CurrentUser([FromRoute] string email)
        {
            try
            {
                var currentUser = await adminService.GetCurrentUserAsync(email);
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
                var roles = await adminService.GetRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModelRequest request)
        {
            try
            {
                var result = await adminService.UpdateUserAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Передача id в контроллер
        [HttpGet("getCurrentNote/{id}")]
        public async Task<IActionResult> CurrentNote([FromRoute] string id)
        {
            try
            {
                var currentNote = await adminService.GetCurrenNoteAsync(Convert.ToInt32(id));
                return Ok(currentNote);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateCurrentNote/{id}")]
        public async Task<IActionResult> UpdateNote([FromRoute] int id, NoteRequest request)
        {
            try
            {
                var response = await adminService.UpdateNoteAsync(id, request);
                if (response)
                {
                    return Ok(response);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deleteCurrentNote/{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] string id)
        {
            try
            {
                await adminService.DeleteNoteAsync(Convert.ToInt32(id));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
