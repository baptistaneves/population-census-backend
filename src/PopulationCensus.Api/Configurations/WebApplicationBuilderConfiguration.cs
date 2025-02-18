namespace PopulationCensus.Api.Configurations;

public class WebApplicationBuilderConfiguration : IWebApplicationBuilderRegister
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var assembly = typeof(Program).Assembly;

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCarter();

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        builder.Services.AddValidatorsFromAssembly(assembly);

        var cors = new Cors();

        builder.Configuration.Bind(nameof(Cors), cors);

        builder.Services.Configure<Cors>(builder.Configuration.GetSection(nameof(Cors)));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins(cors.ClientUrl)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .WithExposedHeaders("Content-Disposition"));
        });
    }
}