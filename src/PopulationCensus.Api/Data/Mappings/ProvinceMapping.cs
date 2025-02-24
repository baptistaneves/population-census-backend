namespace PopulationCensus.Api.Data.Mappings;

public class ProvinceMapping : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.HasMany(x => x.Municipalities)
            .WithOne(x => x.Province)
            .HasForeignKey(x => x.ProvinceId);

        builder.ToTable("Provinces");
    }
}