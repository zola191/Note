namespace Notebook.Server.Exceptions
{
    public class UserNotFoundException :Exception
    {
        public override string Message => "Пользователь не найден";
    }
}
