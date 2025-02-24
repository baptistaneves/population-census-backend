namespace PopulationCensus.Api.Features.Users.GetAll;

public record GetAllUsersQuery() : ICommand<IEnumerable<UserDto>>;

public class GetAllUsersHandler(IApplicationDbContext context) : ICommandHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .AsNoTracking()
            .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
            .Select(x => new UserDto(x.Id, x.UserName, x.Email, x.UserRoles.Select(x => x.Role.Name).FirstOrDefault()!, x.PhoneNumber))
            .ToListAsync(cancellationToken);

        return users;
    }
}