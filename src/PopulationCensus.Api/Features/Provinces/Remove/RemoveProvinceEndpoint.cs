namespace PopulationCensus.Api.Features.Provinces.Remove;

public class RemoveProvinceEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/province/remove/{id}", async (Guid id, ISender sender) =>
        {
            var command = new RemoveProvinceCommand(id);

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("RemoveProvince")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Remove Province")
        .WithDescription("Remove Province");
    }
}
