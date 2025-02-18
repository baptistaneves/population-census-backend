namespace PopulationCensus.Api.Entities;

public class Province : Entity
{
    public string Name { get; private set; }

    public static Province Create(string name)
    {
        return new Province
        {
            Name = name,
        };
    }

    public void Update(string name) => Name = name;
}