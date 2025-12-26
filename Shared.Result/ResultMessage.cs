namespace Shared.Result
{
    public class ResultMessage
    {
        public record Result<T>(bool IsSuccess, T? Objet, string? Error)
        {
            public bool IsFailure => !IsSuccess;

            public static Result<T> Success(T objet) => new(true, objet, null);

            public static Result<T> Failure(string error) => new(false, default, error);
        }
    }
}