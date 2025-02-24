namespace PopulationCensus.Api.Data.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
           .HasMany(user => user.UserRoles)
           .WithOne(userRole => userRole.User)
           .HasForeignKey(userRole => userRole.UserId);
    }
}