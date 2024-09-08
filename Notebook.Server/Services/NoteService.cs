using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Notebook.Server.Data;
using Notebook.Server.Domain;
using Notebook.Server.Dto;
using System.Reflection;

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
            var newNote = mapper.Map<Note>(request);

            newNote.Id = id;
            newNote.UserId = email;

            var existingNote = await dbContext.Notebooks.FirstOrDefaultAsync(f => f.Id == id);

            if (existingNote == null)
            {
                return null;
            }

            var existingNotetype = existingNote.GetType();
            var newNotetype = newNote.GetType();
            var existingNoteProps = existingNotetype.GetProperties();

            var changesFields = existingNoteProps
                .Where(field => !Equals(field.GetValue(existingNote), field.GetValue(newNote)));

            if (changesFields.Count() > 0)
            {


                var logResult = new Dictionary<string, (string,string) >();

                foreach (var field in changesFields)
                {
                    logResult[field.Name] = (field.GetValue(existingNote).ToString(), field.GetValue(newNote).ToString());
                }

                var noteChangeLog = new NoteChangeLog()
                {
                    ChangedAt = DateTime.UtcNow,
                    Email = email,
                    Log = $"Пользователь {email} изменил Note: \n" + string.Join(", ", logResult.Select((f,g) =>
                    {
                        return $"{g+1}. Поле {f.Key} было значение {f.Value.Item1}, стало {f.Value.Item2} \n";
                    }))
                };
                dbContext.NoteChangeLogs.Add(noteChangeLog);
            }

            dbContext.Entry(existingNote).CurrentValues.SetValues(newNote);

            await dbContext.SaveChangesAsync();
            var response = mapper.Map<NoteModel>(newNote);

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
