using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Data;
using Notebook.Server.Domain;
using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public class NotebookService : INotebookService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public NotebookService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<NoteModel> CreateAsync(NoteRequest request, string email)
        {
            var notebook = mapper.Map<Note>(request);

            notebook.UserId = email;

            await dbContext.AddAsync(notebook);
            await dbContext.SaveChangesAsync();

            var response = mapper.Map<NoteModel>(notebook);
            return response;
        }

        public async Task<IEnumerable<NoteModel>> GetAllAsync(string email)
        {
            var notebooks = await dbContext.Notebooks.Where(f => f.UserId == email).ToListAsync();
            var response = mapper.Map<List<NoteModel>>(notebooks);
            return response;
        }

        public async Task<NoteModel> GetById(int id, string email)
        {
            var existingNotebook = await dbContext.Notebooks.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);
            if (existingNotebook.User.Email != email)
            {
                return null;
            }
            var response = mapper.Map<NoteModel>(existingNotebook);
            return response;
        }

        public async Task<NoteModel> UpdateAsync(int id, NoteRequest request, string email)
        {
            var notebook = mapper.Map<Note>(request);

            notebook.Id = id;
            notebook.UserId = email;

            var existingNotebook = await dbContext.Notebooks.FirstOrDefaultAsync(f => f.Id == id);
            if (existingNotebook == null)
            {
                return null;
            }

            dbContext.Entry(existingNotebook).CurrentValues.SetValues(notebook);
            await dbContext.SaveChangesAsync();
            var response = mapper.Map<NoteModel>(notebook);

            return response;
        }

        public async Task<NoteModel> DeleteAsync(int id)
        {
            var existingNotebook = await dbContext.Notebooks.FirstOrDefaultAsync(f => f.Id == id);
            if (existingNotebook == null)
            {
                return null;
            }

            dbContext.Notebooks.Remove(existingNotebook);
            await dbContext.SaveChangesAsync();

            var response = mapper.Map<NoteModel>(existingNotebook);
            return response;
        }

    }
}
