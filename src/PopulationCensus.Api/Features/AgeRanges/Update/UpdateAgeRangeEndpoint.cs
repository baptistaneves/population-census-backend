using Mapster;

namespace PopulationCensus.Api.Features.Provinces.Update;

public record UpdateAgeRangeRequest(Guid Id, string Range, string Description);

public class UpdateAgeRangeEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/ageRange/update", async ([FromBody] UpdateAgeRangeRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateAgeRangeCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("UpdateAgeRange")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Age Range")
        .WithDescription("Update Age Range")
        .WithOpenApi();
    }
}