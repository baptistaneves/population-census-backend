namespace PopulationCensus.Api.Configurations;

public class DependencyInjectionConfiguration : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        builder.Services.AddScoped<IJwtService, JwtService>();

    }
}