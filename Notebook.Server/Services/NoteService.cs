using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Data;
using Notebook.Server.Domain;
using Notebook.Server.Dto;

namespace Notebook.Server.Services
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public NoteService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<NoteModel> CreateAsync(NoteRequest request, string email)
        {
            var note = mapper.Map<Note>(request);

            note.UserId = email;

            await dbContext.AddAsync(note);
            await dbContext.SaveChangesAsync();

            var response = mapper.Map<NoteModel>(note);
            return response;
        }

        public async Task<IEnumerable<NoteModel>> GetAllAsync(string email)
        {
            var existingGoogleEmail = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            var notes = new List<Note>();
            if (existingGoogleEmail == null)
            {
                notes = await dbContext.Notebooks.Where(f => f.UserId == email).ToListAsync();
            }
            else
            {
                notes = await dbContext.Notebooks.Where(f => f.User.Email == email).ToListAsync();
            }
            var response = mapper.Map<List<NoteModel>>(notes);
            return response;
        }

        public async Task<NoteModel> GetById(int id, string email)
        {
            var existingNote = await dbContext.Notebooks.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);
            if (existingNote.User.Email != email)
            {
                return null;
            }
            var response = mapper.Map<NoteModel>(existingNote);
            return response;
        }

        public async Task<NoteModel> UpdateAsync(int id, NoteRequest request, string email)
        {
            var note = mapper.Map<Note>(request);

            note.Id = id;
            note.UserId = email;

            var existingNote = await dbContext.Notebooks.FirstOrDefaultAsync(f => f.Id == id);
            if (existingNote == null)
            {
                return null;
            }

            dbContext.Entry(existingNote).CurrentValues.SetValues(note);
            await dbContext.SaveChangesAsync();
            var response = mapper.Map<NoteModel>(note);

            return response;
        }

        public async Task<NoteModel> DeleteAsync(int id)
        {
            var existingNote = await dbContext.Notebooks.FirstOrDefaultAsync(f => f.Id == id);
            if (existingNote == null)
            {
                return null;
            }

            dbContext.Notebooks.Remove(existingNote);
            await dbContext.SaveChangesAsync();

            var response = mapper.Map<NoteModel>(existingNote);
            return response;
        }

        public async Task<List<NoteModel>> CreateNotesAsync(List<Note> notes, string email)
        {
            foreach (var note in notes)
            {
                note.UserId = email;
            }

            await dbContext.AddRangeAsync(notes);
            await dbContext.SaveChangesAsync();

            var response = mapper.Map<List<NoteModel>>(notes);
            return response;
        }
    }
}
