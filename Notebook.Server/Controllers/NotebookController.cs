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
        public async Task<IActionResult> CreateNotebook([FromBody] NoteRequest requestDto)
        {
            var existingAccount = notebookService.CreateAsync(requestDto);
            if (existingAccount == null)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{id:int}")]
        // обезопасить получение данных
        public async Task<IActionResult> GetNotebookById([FromRoute] int id)
        {
            var response = await notebookService.GetById(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateNotebook([FromRoute] int id, NoteRequest requestDto)
        {
            var response = await notebookService.UpdateAsync(id, requestDto);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteNotebook(int id) // проверить [fromRoute]
        {
            await notebookService.DeleteAsync(id);
            return Ok();

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetNotebooks()
        {
            var email = accountService.GetUserEmail(Request);

            var notebooks = await notebookService.GetAllAsync(email);
            return Ok(notebooks);
        }

    }
}
