namespace QuizIdentity.Application.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message = "У вас нет доступа к этому ресурсу.") : base(message)
    {
    }
}
