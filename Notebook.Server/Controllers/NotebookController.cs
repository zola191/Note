using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Dto;
using Notebook.Server.Services;

namespace Notebook.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class NotebookController : ControllerBase
    {
        private readonly INotebookService notebookService;
        private readonly IAccountService accountService;

        public NotebookController(INotebookService notebookService, IAccountService accountService)
        {
            this.notebookService = notebookService;
            this.accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NoteRequest request)
        {
            var email = accountService.GetUserEmail(Request);
            var existingAccount = await notebookService.CreateAsync(request,email);
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
            var email = accountService.GetUserEmail(Request);

            var response = await notebookService.GetById(id,email);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, NoteRequest requestDto)
        {
            var email = accountService.GetUserEmail(Request);
            var response = await notebookService.UpdateAsync(id, requestDto, email);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await notebookService.DeleteAsync(id);
            return Ok();

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var email = accountService.GetUserEmail(Request);
            var notebooks = await notebookService.GetAllAsync(email);
            return Ok(notebooks);
        }

    }
}
