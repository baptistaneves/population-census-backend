﻿namespace PopulationCensus.Api.Entities;

public class Role : IdentityRole<Guid>
{
    public IEnumerable<UserRole> UserRoles { get; set; }
}
