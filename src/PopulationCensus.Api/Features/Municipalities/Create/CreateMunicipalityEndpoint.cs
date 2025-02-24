using Mapster;

namespace PopulationCensus.Api.Features.Provinces.Create;

public record CreateMunicipalityRequest(string Name, Guid ProvinceId);

public class CreateMunicipalityEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/municipality/create", async ([FromBody] CreateMunicipalityRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateMunicipalityCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("CreateMunicipality")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Municipality")
        .WithDescription("Create Municipality")
        .WithOpenApi();
    }
}