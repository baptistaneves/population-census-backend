namespace PopulationCensus.Api.Features.Roles.GetAll;

public class GetAllRolesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/role/get-all", async (ISender sender) =>
        {
            return Results.Ok(RoleList.GetRoles());
        })
        .WithName("GetAllRoles")
        .Produces(StatusCodes.Status200OK)
        .WithDescription("Get All Roles")
        .WithOpenApi();
    }
}

public class RoleList
{
    public const string Administrator = "Administrador";
    public const string Technician = "Técnico";

    public static List<string> GetRoles()
    {
        return new List<string> 
        {
            Administrator,
            Technician
        };
    }
}