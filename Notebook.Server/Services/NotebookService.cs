using AutoMapper;
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

        public async Task<NoteModel> CreateAsync(NoteRequest request)
        {
            var notebook = mapper.Map<Note>(request);

            await dbContext.AddAsync(notebook);
            await dbContext.SaveChangesAsync();

            var response = mapper.Map<NoteModel>(notebook);
            return response;
        }

        public async Task<IEnumerable<NoteModel>> GetAllAsync()
        {
            var notebooks = await dbContext.Notebooks.ToListAsync();
            var response = new List<NoteModel>();
            notebooks.ForEach(f =>
            {
                var item = mapper.Map<NoteModel>(f);
                response.Add(item);
            });
            return response;
        }

        public async Task<NoteModel> GetById(int id)
        {
            var existingNotebook = await dbContext.Notebooks.FirstOrDefaultAsync(f => f.Id == id);

            var response = mapper.Map<NoteModel>(existingNotebook);
            return response;
        }

        public async Task<NoteModel> UpdateAsync(int id, NoteRequest request)
        {
            var notebook = mapper.Map<Note>(request);
            notebook.Id = id;

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
