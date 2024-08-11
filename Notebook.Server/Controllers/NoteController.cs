using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Dto;
using Notebook.Server.Services;
using Notebook.Server.Validations;

namespace Notebook.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class NoteController : ControllerBase
    {
        private readonly INoteService noteService;
        private readonly IUserService userService;

        public NoteController(INoteService noteService, IUserService userService)
        {
            this.noteService = noteService;
            this.userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NoteRequest request)
        {
            var validator = new NoteRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var email = userService.GetUserEmail(Request);
            var existingAccount = await noteService.CreateAsync(request, email);
            if (existingAccount == null)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{id:int}")]
        // обезопасить получение данных
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var email = userService.GetUserEmail(Request);

            var response = await noteService.GetById(id, email);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, NoteRequest requestDto)
        {
            var email = userService.GetUserEmail(Request);
            var response = await noteService.UpdateAsync(id, requestDto, email);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await noteService.DeleteAsync(id);
            return Ok();

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
