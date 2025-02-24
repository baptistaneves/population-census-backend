namespace PopulationCensus.Api.Features.Provinces.Remove;

public class RemoveAgeRangeEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/ageRange/remove/{id}", async (Guid id, ISender sender) =>
        {
            var command = new RemoveAgeRangeCommand(id);

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("RemoveAgeRange")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Remove Age Range")
        .WithDescription("Remove Age Range");
    }
}
