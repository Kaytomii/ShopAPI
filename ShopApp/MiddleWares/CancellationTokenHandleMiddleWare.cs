namespace ShopApi.MiddleWares;

public class CancellationTokenHandleMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CancellationTokenHandleMiddleWare> _logger;

    public CancellationTokenHandleMiddleWare(RequestDelegate next, ILogger<CancellationTokenHandleMiddleWare> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception _) when (_ is OperationCanceledException)
        {
            _logger.LogError("Request Cancelled");
        }
    }
}
