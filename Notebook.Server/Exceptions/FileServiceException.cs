namespace Notebook.Server.Exceptions
{
    public class FileServiceException : Exception
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }

        public FileServiceException(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
