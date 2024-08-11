
namespace Notebook.Server.Exceptions
{
    public class NoteListFieldException : FileServiceException
    {
        public List<string> Errors { get; set; }
        public NoteListFieldException(List<string> errores) 
            : base("В загруженном файле имеется список ошибок", 401)
        {
            Errors = errores;
        }

        //public override string Message => "Задан неправильный формат записи";
        //public List<string> errors;

        //public NoteListFieldException(List<string> errors)
        //{
        //    this.errors = errors;
        //}

        //public NoteListFieldException()
        //{

        //}
        //public NoteListFieldException(string message)
        //    : base(message)
        //{

        //}

    }
}
