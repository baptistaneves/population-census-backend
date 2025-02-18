namespace PopulationCensus.Api.Configurations;

public class DbConfiguration : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        string mySqlConnection = builder.Configuration.GetConnectionString("Database")!;

        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));
    }
}