using Mapster;

namespace PopulationCensus.Api.Features.Users.Create;

public record CreateUserRequest(string Email, string UserName, string PhoneNumber, string Role, string Password);

public class CreateUserEndpoint : BaseEndpoint, ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/user/create", async ([FromBody] CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUserCommand>();

            var result = await sender.Send(command);

            return Response(result);
        })
        .WithName("CreateUser")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Create User")
        .WithOpenApi();
    }
}