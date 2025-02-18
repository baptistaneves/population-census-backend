using Mapster;

namespace PopulationCensus.Api.Features.Provinces.Update;

public record UpdateProvinceRequest(Guid Id, string Name);

public class UpdateProvinceEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/province/update", async ([FromBody] UpdateProvinceRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProvinceCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("UpdateProvince")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Province")
        .WithDescription("Update Province")
        .WithOpenApi();
    }
}