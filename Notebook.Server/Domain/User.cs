namespace Notebook.Server.Domain
{
    public class User
    {
        public string Email { get; set; }
        public string? Salt { get; set; }
        public string? PasswordHash { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        //one to many relationship with Notes
        public List<Note> Notes { get; set; }
        public List<Role> Roles { get; set; }
        public User()
        {
            Notes = new List<Note>();
            Roles = new List<Role>();
        }
    }

    // Изменить класс Note таким образом чтобы в рамках в контекста базы данных получилась
    // user - account (1-1)
    // user - notes (1-много)
    // при создании нового пользователя логика след. 
    // 1. Создать 1 экз. класса аккаунт
    // 2. Если пользователя нет, то создать новый экземпляр класс user
    // 3. Ссылку аккаунта кладем в свойство класса user
    // 4. Получившейся объект user сохраняем в БД в рамках результа создается учетная запись user и account
    // 5. В notebookcontroller создать не просто запись, а связать запись с пользователем и его токеном 

}
