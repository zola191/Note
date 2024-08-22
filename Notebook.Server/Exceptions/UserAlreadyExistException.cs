namespace Notebook.Server.Exceptions
{
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException(string exception) : base(exception)
        {

        }
    }
}
