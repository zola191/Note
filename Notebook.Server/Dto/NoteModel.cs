namespace Notebook.Server.Dto
{
    public class NoteModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; } //(поле не является обязательным);
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public DateTime BirthDay { get; set; } //(поле не является обязательным);
        public string Organization { get; set; } //(поле не является обязательным);
        public string Position { get; set; } //(поле не является обязательным);
        public string Other { get; set; } //(поле не является обязательным);
    }
}
