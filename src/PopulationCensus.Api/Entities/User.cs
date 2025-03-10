﻿namespace PopulationCensus.Api.Entities;

public class User : IdentityUser<Guid>
{
    public IEnumerable<UserRole> UserRoles { get; set; }
}