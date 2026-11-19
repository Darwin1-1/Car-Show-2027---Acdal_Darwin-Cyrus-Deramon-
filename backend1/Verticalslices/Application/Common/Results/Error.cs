namespace Application.Common.Results;

public enum ErrorType
{
    Conflict,
    Validation,
    NotFound,
    Failure
}

public sealed record Error(string Code, string Description, ErrorType Type)
{
    public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);

    public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);
}
