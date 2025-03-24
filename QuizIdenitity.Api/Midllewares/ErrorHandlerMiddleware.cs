using System.Net;
using System.Text.Json;
using QuizIdentity.Application.Exceptions;
using QuizIdentity.Application.Wrapers;

namespace QuizIdenitity.Api.Midllewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

            _logger.LogError(error, "Произошла ошибка: {Message}", error?.Message);

            switch (error)
            {
                case ApiException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case QuizIdentity.Application.Exceptions.ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Errors = e.Errors;
                    break;

                case KeyNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ForbiddenAccessException e:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;

                case InvalidOperationException e:
                    response.StatusCode = (int)HttpStatusCode.Conflict; 
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}
