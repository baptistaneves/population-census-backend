namespace PopulationCensus.Api.Data;

public interface IApplicationDbContext
{
    public DbSet<Province> Provinces { get; }
    public DbSet<Municipality> Municipalities { get; }
    public DbSet<AgeRange> AgeRanges { get; }
    public DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}