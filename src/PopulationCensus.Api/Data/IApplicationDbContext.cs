namespace PopulationCensus.Api.Data;

public interface IApplicationDbContext
{
    public DbSet<Province> Provinces { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}