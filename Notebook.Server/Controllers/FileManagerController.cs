using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Dto;
using Notebook.Server.Exceptions;
using Notebook.Server.Services;

namespace Notebook.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FileManagerController : ControllerBase
    {
        private readonly IFileService fileService;
        private readonly INoteService noteService;
        private readonly IUserService userService;
        public FileManagerController(IFileService fileService, INoteService noteService, IUserService userService)
        {
            this.fileService = fileService;
            this.noteService = noteService;
            this.userService = userService;
        }

        [HttpPost("UploadFromExcel")]
        public async Task<IActionResult> UploadFromExcel(IFormFile file)
        {
            try
            {
                var notes = fileService.DeserializeFromFile(file);
                var email = userService.GetUserEmail(Request);
                var response = await noteService.CreateNotesAsync(notes, email);
                return Ok(response);
            }
            catch (NoteListFieldException ex)
            {
                var errorModels = ex.Errors.Select(f => new ErrorModel(f)).ToArray();
                return BadRequest(errorModels);
            }

            //catch (Exception ex)
            //{
            //    return BadRequest(new ErrorModel(ex.Message));
            //}

        }
    }
}
