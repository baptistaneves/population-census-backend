namespace PopulationCensus.Api.Dtos;

public record MunicipalityDto(Guid Id, string Name, string ProvinceName, Guid ProvinceId);