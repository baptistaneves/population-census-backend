namespace PopulationCensus.Api.Features.Provinces.GetAll;

public record GetAllAgeRangesQuery : IQuery<IEnumerable<AgeRange>>;

public class GetAllAgeRangesHandler(IApplicationDbContext context) : IQueryHandler<GetAllAgeRangesQuery, IEnumerable<AgeRange>>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<IEnumerable<AgeRange>> Handle(GetAllAgeRangesQuery query, CancellationToken cancellationToken)
    {
        return await _context.AgeRanges.AsNoTracking().ToListAsync(cancellationToken);
    }
}