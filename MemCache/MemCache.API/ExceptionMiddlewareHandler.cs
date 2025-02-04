namespace MemCache.API
{
    public class ExceptionMiddlewareHandler
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddlewareHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                using(StreamWriter sw = new StreamWriter(""))
                {
                    sw.WriteLine(exception.ToString());
                }
            }
        }
    }
}
