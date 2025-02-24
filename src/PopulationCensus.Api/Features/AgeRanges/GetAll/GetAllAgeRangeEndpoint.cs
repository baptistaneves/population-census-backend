namespace PopulationCensus.Api.Features.Provinces.GetAll;

public class GetAllAgeRangeEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ageRange/get-all", async (ISender sender) =>
        {
            var query = new GetAllAgeRangesQuery();

            var response = await sender.Send(query);

            return Results.Ok(response);
        })
        .WithName("GetAllAgeRanges")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Get All Age Ranges")
        .WithDescription("Get All Age Ranges")
        .WithOpenApi();
    }
}