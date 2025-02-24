namespace PopulationCensus.Api.Entities;

public class AgeRange : Entity
{
    public string Range { get; set; }
    public string Description { get; private set; }

    public static AgeRange Create(string range, string description)
    {
        return new AgeRange
        {
            Range = range,
            Description = description
        };
    }

    public void Update(string range, string description)
    {
        Range = range;
        Description = description;
    }
}
