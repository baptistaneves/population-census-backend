namespace PopulationCensus.Api.Data.Mappings;

public class AgeRangeMapping : IEntityTypeConfiguration<AgeRange>
{
    public void Configure(EntityTypeBuilder<AgeRange> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Range)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(e => e.Description)
           .HasColumnType("varchar(50)")
           .IsRequired();

        builder.ToTable("AgeRanges");
    }
}