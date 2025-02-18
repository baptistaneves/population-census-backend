using Mapster;

namespace PopulationCensus.Api.Features.Provinces.Create;

public record CreateProvinceRequest(string Name);

public class CreateProvinceEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/province/create", async ([FromBody] CreateProvinceRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProvinceCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("CreateProvince")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Province")
        .WithDescription("Create Province")
        .WithOpenApi();
    }
}