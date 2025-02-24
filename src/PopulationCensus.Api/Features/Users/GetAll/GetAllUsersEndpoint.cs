namespace PopulationCensus.Api.Features.Users.GetAll;

public record GetAllUsersResponse(IEnumerable<User> Users);

public class GetAllUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user/get-all", async (ISender sender) =>
        {
            var users = await sender.Send(new GetAllUsersQuery());

            return Results.Ok(users);
        })
        .WithName("GetAllUsers")
        .Produces<GetAllUsersResponse>(StatusCodes.Status200OK)
        .WithDescription("Get All Users")
        .WithOpenApi();
    }
}