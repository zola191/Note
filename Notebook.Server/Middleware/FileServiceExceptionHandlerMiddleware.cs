using Notebook.Server.Exceptions;

namespace Notebook.Server.Middleware
{
    public class FileServiceExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case FileServiceException fileServiceException:
                        context.Response.StatusCode = fileServiceException.StatusCode;
                        await context.Response.WriteAsync(fileServiceException.Message);
                        break;

                    case FileNotFoundException fileNotFoundException:
                        context.Response.StatusCode = 402;
                        await context.Response.WriteAsync(fileNotFoundException.Message);
                        break;

                    case FormatException formatException:
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync(formatException.Message);
                        break;

                    default:
                        // Обработка других исключений, если необходимо
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected error occurred.");
                        break;
                }
            }
        }
    }
}
