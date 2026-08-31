using System.Diagnostics;

namespace Dot.Net.WebApi.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string user = context.User.Identity?.IsAuthenticated == true
            ? (context.User.FindFirst("sub")?.Value ?? "inconnu")
            : "anonyme";

        Stopwatch stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Requete {Method} {Path} par {User}",
            context.Request.Method, context.Request.Path, user);

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation("Reponse {StatusCode} pour {Method} {Path} par {User} en {Elapsed} ms",
            context.Response.StatusCode, context.Request.Method, context.Request.Path,
            user, stopwatch.ElapsedMilliseconds);
    }
}