namespace PopulationCensus.Api.Dtos;

public record UserDto(Guid Id, string UserName, string Email, string Role, string PhoneNumber);