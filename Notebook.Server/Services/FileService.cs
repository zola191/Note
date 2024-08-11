using ExcelDataReader;
using Notebook.Server.Domain;
using Notebook.Server.Dto;
using Notebook.Server.Exceptions;
using Notebook.Server.Validations;

namespace Notebook.Server.Services
{
    public class FileService : IFileService
    {
        public List<string> Errors { get; private set; } = new();
        public List<Note> DeserializeFromFile(IFormFile file)
        {
            
            if (file == null && file.Length == 0)
            {
                Errors.Add("Файл не найден");
                throw new FileNotFoundException("Файл не найден");
            }

            if (file.FileName.Split('.').Last() != "xlsx")
            {
                Errors.Add("Получен некорретный формат загружаемого файла");
                throw new FormatException("Получен некорретный формат загружаемого файла");
            }

            var notes = new List<Note>();

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                using (var reader = ExcelReaderFactory.CreateReader(ms))
                {
                    // skip header of excel table
                    
                    reader.Read();
                    do
                    {
                        var index = 0;
                        while (reader.Read())
                        {
                            var note = new Note();
                            note.FirstName = reader.GetValue(0).ToString();
                            note.MiddleName = reader.GetValue(1).ToString();
                            note.LastName = reader.GetValue(2).ToString();
                            note.PhoneNumber = reader.GetValue(3).ToString();
                            note.Country = reader.GetValue(4).ToString();

                            var isDate = DateTime.TryParse(reader.GetValue(5).ToString(), out var result);
                            
                            if (!isDate)
                            {
                                Errors.Add($"Неправильно указана дата в строке {index}");
                            }
                            note.BirthDay = result;
                            note.Organization = reader.GetValue(6).ToString();
                            note.Position = reader.GetValue(7).ToString();
                            note.Other = reader.GetValue(8).ToString();

                            var validator = new NoteFromFileValidator();
                            var validationResult = validator.Validate(note);

                            if (!validationResult.IsValid)
                            {
                                //Errors.Add($"Запись под номером {index} сформирована не по шаблону");
                                Errors.AddRange(validationResult.Errors.Select(f => $"Ошибка в записи {index}" + f.ErrorMessage));
                            }

                            notes.Add(note);
                            index++;
                        }
                        
                    } while (reader.NextResult());
                }
            }

            if (notes.Count == 0)
            {
                throw new FileLoadException("Был загружен пустой файл");
            }

            if (Errors.Count != 0)
            {
                throw new NoteListFieldException(Errors);
            }

            return notes;
        }
    }
}
