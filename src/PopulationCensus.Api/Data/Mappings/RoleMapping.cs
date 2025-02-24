namespace PopulationCensus.Api.Data.Mappings;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
           .HasMany(role => role.UserRoles)
           .WithOne(userRole => userRole.Role)
           .HasForeignKey(userRole => userRole.RoleId);
    }
}