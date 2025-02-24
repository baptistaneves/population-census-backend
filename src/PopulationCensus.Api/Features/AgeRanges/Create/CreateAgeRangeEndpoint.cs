using Mapster;

namespace PopulationCensus.Api.Features.Provinces.Create;

public record CreateAgeRangeRequest(string Range, string Description);

public class CreateAgeRangeEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ageRange/create", async ([FromBody] CreateAgeRangeRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProvinceCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("CreateAgeRange")
        .Produces<Result<bool>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Age Range")
        .WithDescription("Create Age Range")
        .WithOpenApi();
    }
}