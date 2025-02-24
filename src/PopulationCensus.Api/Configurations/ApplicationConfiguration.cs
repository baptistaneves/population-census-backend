using PopulationCensus.Api.Data.Extensions;

namespace PopulationCensus.Api.Configurations;

public class ApplicationConfiguration : IWebApplicationRegister
{
    public void RegisterPipelineComponents(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.InitialiseDatabaseAsync().GetAwaiter().GetResult();

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowOrigin");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapCarter();
    }
}