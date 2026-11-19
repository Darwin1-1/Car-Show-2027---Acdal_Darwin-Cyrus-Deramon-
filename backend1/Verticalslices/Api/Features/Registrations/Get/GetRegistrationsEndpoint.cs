using Application.Features.Registrations.Get;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Api.Features.Registrations.Get;

public static class GetRegistrationsEndpoint
{
    public static RouteGroupBuilder MapGetRegistrationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/registrations");
        group.MapGet("", HandleAsync)
            .WithName("GetRegistrations")
            .WithTags("Registrations")
            .Produces<GetRegistrationsResponse>()
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        return group;
    }

    private static async Task<IResult> HandleAsync(
        [AsParameters] GetRegistrationsRequest request,
        IValidator<GetRegistrationsQuery> validator,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetRegistrationsQuery(
            request.ParticipantName,
            request.Type,
            request.Status,
            request.Page,
            request.PageSize);

        var validation = await validator.ValidateAsync(query, cancellationToken);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Select(error => error.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors);
        }

        var response = await sender.Send(query, cancellationToken);
        return Results.Ok(response);
    }
}
