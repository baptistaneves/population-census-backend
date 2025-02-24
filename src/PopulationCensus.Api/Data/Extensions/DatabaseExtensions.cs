namespace PopulationCensus.Api.Data.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        await context.Database.MigrateAsync();

        await SeedAsync(context, userManager, roleManager);
    }

    private static async Task SeedAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager);
    }

    private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
    {
        foreach (var role in InitialData.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    private static async Task SeedUsersAsync(UserManager<User> userManager)
    {
        foreach (var user in InitialData.Users)
        {
            if (await userManager.FindByNameAsync(user.UserName!) == null)
            {
                await userManager.CreateAsync(user, "Senha123");
                await userManager.AddToRoleAsync(user, RoleList.Administrator);
            }
        }
    }
}

internal class InitialData
{
    public static List<User> Users => new()
    {
        new User
        {
            UserName = "maria.joana",
            Email = "maria.joana@email.com",
            PhoneNumber = "934569038"
        }
    };

    public static List<Role> Roles => new()
    {
        new Role { Name = RoleList.Administrator }
    };
}