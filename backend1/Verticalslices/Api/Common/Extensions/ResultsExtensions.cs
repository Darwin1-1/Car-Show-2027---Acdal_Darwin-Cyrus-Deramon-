using Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common.Extensions;

public static class ResultsExtensions
{
    public static IResult ToCreatedResult<T>(this Result<T> result, Func<T, IResult> onSuccess)
    {
        if (result.IsSuccess)
        {
            if (result.Value is null)
            {
                throw new InvalidOperationException("A successful result must contain a value.");
            }

            return onSuccess(result.Value);
        }

        var error = result.Error
            ?? throw new InvalidOperationException("A failed result must contain an error.");

        return error.Type switch
        {
            ErrorType.Conflict => Results.Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = error.Code,
                Detail = error.Description
            }),
            ErrorType.Validation => Results.BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = error.Code,
                Detail = error.Description
            }),
            _ => Results.Problem(
                title: error.Code,
                detail: error.Description,
                statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}
