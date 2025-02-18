namespace PopulationCensus.Api.Features.Provinces.GetAll;

public record GetAllProvincesQuery : IQuery<IEnumerable<Province>>;

public class GetAllProvincesHandler(IApplicationDbContext context) : IQueryHandler<GetAllProvincesQuery, IEnumerable<Province>>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<IEnumerable<Province>> Handle(GetAllProvincesQuery query, CancellationToken cancellationToken)
    {
        return await _context.Provinces.AsNoTracking().ToListAsync(cancellationToken);
    }
}