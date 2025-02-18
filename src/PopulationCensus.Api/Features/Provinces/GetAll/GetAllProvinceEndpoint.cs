namespace PopulationCensus.Api.Features.Provinces.GetAll;

public class GetAllProvinceEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/province/get-all", async (ISender sender) =>
        {
            var query = new GetAllProvincesQuery();

            var response = await sender.Send(query);

            return Results.Ok(response);
        })
        .WithName("GetAllProvinces")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Get All Provinces")
        .WithDescription("Get All Provinces")
        .WithOpenApi();
    }
}