namespace PopulationCensus.Api.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid,
    IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IApplicationDbContext
{
    public DbSet<Province> Provinces => Set<Province>();
    public DbSet<Municipality> Municipalities => Set<Municipality>();
    public DbSet<AgeRange> AgeRanges => Set<AgeRange>();
    public DbSet<User> Users => Set<User>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}