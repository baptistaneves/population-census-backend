namespace PopulationCensus.Api.Extensions;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {errorMessage}, Time of occurrence {time}", exception.Message, DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) details = exception switch
        {
            UnauthorizedAccessException =>
            (
                "Você não tem autorização para acessar esse recurso.",
                "Não Autorizado",
                StatusCodes.Status401Unauthorized
            ),
            _ =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("TraceId", context.TraceIdentifier);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = details.StatusCode;

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}