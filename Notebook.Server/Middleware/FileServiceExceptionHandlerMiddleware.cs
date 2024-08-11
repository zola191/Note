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
            catch (FileServiceException e)
            {
                context.Response.StatusCode = e.StatusCode;
                await context.Response.WriteAsync(e.Message);
            }
            catch(FileNotFoundException e)
            {
                context.Response.StatusCode = 402;
                await context.Response.WriteAsync(e.Message);
            }
            catch(FormatException e)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}
