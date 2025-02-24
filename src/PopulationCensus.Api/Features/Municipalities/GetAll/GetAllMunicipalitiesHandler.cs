namespace PopulationCensus.Api.Features.Provinces.GetAll;

public record GetAllMunicipalitiesQuery : IQuery<IEnumerable<MunicipalityDto>>;

public class GetAllMunicipalitiesHandler(IApplicationDbContext context) : IQueryHandler<GetAllMunicipalitiesQuery, IEnumerable<MunicipalityDto>>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<IEnumerable<MunicipalityDto>> Handle(GetAllMunicipalitiesQuery query, CancellationToken cancellationToken)
    {
        return await _context.Municipalities
            .AsNoTracking()
            .Include(x => x.Province)
            .Select(x => new MunicipalityDto(x.Id, x.Name, x.Province!.Name, x.Province.Id))
            .ToListAsync(cancellationToken);
    }
}