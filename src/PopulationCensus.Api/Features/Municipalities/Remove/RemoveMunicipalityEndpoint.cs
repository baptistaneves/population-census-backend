namespace PopulationCensus.Api.Features.Provinces.Remove;

public class RemoveMunicipalityEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/municipality/remove/{id}", async (Guid id, ISender sender) =>
        {
            var command = new RemoveMunicipalityCommand(id);

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("RemoveMunicipality")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Remove Municipality")
        .WithDescription("Remove Municipality");
    }
}
