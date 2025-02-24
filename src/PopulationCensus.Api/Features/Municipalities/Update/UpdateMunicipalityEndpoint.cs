using Mapster;

namespace PopulationCensus.Api.Features.Provinces.Update;

public record UpdateMunicipalityRequest(Guid Id, string Name, Guid ProvinceId);

public class UpdateMunicipalityEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/municipality/update", async ([FromBody] UpdateMunicipalityRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateMunicipalityCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("UpdateMunicipality")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Municipality")
        .WithDescription("Update Municipality")
        .WithOpenApi();
    }
}