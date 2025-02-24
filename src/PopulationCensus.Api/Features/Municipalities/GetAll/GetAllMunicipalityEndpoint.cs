namespace PopulationCensus.Api.Features.Provinces.GetAll;

public class GetAllMunicipalityEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/municipality/get-all", async (ISender sender) =>
        {
            var query = new GetAllMunicipalitiesQuery();

            var response = await sender.Send(query);

            return Results.Ok(response);
        })
        .WithName("GetAllMunicipalities")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Get All Municipalities")
        .WithDescription("Get All Municipalities")
        .WithOpenApi();
    }
}