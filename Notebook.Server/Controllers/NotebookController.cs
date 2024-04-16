using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Dto;
using Notebook.Server.Services;

namespace Notebook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotebookController : ControllerBase
    {
        private readonly INotebookService notebookService;

        public NotebookController(INotebookService notebookService)
        {
            this.notebookService = notebookService;
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

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetNotebookById([FromRoute] int id)
        {
            var response = await notebookService.GetById(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateNotebook([FromRoute] int id, NoteRequest requestDto)
        {
            var response = await notebookService.UpdateAsync(id, requestDto);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteNotebook([FromRoute] int id)
        {
            await notebookService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetNotebooks()
        {
            var notebooks = await notebookService.GetAllAsync();
            return Ok(notebooks);
        }

    }
}
