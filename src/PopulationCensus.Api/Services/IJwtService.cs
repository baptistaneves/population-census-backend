namespace PopulationCensus.Api.Services;

public interface IJwtService
{
    Task<string> GetJwtString(User user);
}