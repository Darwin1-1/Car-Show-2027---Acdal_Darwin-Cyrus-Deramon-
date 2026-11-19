using Api.Common.Extensions;
using Application.Features.Registrations.Create;
using FluentValidation;
using MediatR;

namespace Api.Features.Registrations.Create;

public static class CreateRegistrationEndpoint
{
    public static RouteGroupBuilder MapCreateRegistrationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/registrations");
        group.MapPost("", HandleAsync)
            .WithName("CreateRegistration")
            .WithTags("Registrations")
            .Produces<CreateRegistrationResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);

        return group;
    }

    private static async Task<IResult> HandleAsync(
        CreateRegistrationRequest request,
        IValidator<CreateRegistrationCommand> validator,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateRegistrationCommand(
            request.ParticipantName,
            request.ContactNumber,
            request.Email,
            request.Address,
            request.CarMakeModel,
            request.PlateNumber,
            request.RegistrationType);

        var validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Select(error => error.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors);
        }

        var result = await sender.Send(command, cancellationToken);
        return result.ToCreatedResult(response => Results.Created(
            $"/api/registrations/{response.Id}",
            response));
    }
}
