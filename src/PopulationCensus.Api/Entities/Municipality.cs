namespace PopulationCensus.Api.Entities;

public class Municipality : Entity
{
    public string Name { get; private set; }
    public Guid ProvinceId { get; private set; }

    public Province? Province { get; private set; }

    public static Municipality Create(string name, Guid provinceId)
    {
        return new Municipality
        {
            Name = name,
            ProvinceId = provinceId
        };
    }

    public void Update(string name, Guid provinceId)
    {
        Name = name;
        ProvinceId = provinceId;
    }
}