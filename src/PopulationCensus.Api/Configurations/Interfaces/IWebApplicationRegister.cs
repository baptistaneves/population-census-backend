namespace PopulationCensus.Api.Configurations.Interfaces;

public interface IWebApplicationRegister : IRegister
{
    void RegisterPipelineComponents(WebApplication app);
}
