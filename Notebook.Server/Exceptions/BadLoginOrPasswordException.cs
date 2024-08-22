namespace Notebook.Server.Exceptions
{
    public class BadLoginOrPasswordException : Exception
    {
        public BadLoginOrPasswordException(string exception) : base(exception) 
        {
            
        }

    }
}
