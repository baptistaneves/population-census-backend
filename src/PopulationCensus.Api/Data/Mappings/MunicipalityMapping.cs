namespace PopulationCensus.Api.Data.Mappings;

public class MunicipalityMapping : IEntityTypeConfiguration<Municipality>
{
    public void Configure(EntityTypeBuilder<Municipality> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasColumnType("varchar(90)")
            .IsRequired();

        builder.ToTable("Municipalities");
    }
}