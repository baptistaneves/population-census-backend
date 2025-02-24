namespace PopulationCensus.Api.Features.Provinces.Remove;

public record RemoveMunicipalityCommand(Guid Id) : ICommand<Result<bool>>;

public class RemoveMunicipalityCommandValidation : AbstractValidator<RemoveMunicipalityCommand>
{
    public RemoveMunicipalityCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage(ErrorMessages.IdNotValid)
            .NotEqual(Guid.Empty).WithMessage(ErrorMessages.IdNotValid);
    }
}

public class RemoveMunicipalityHandler
    (IApplicationDbContext context,
     IValidator<RemoveMunicipalityCommand> validator)
    : ICommandHandler<RemoveMunicipalityCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<RemoveMunicipalityCommand> _validator = validator;

    public async Task<Result<bool>> Handle(RemoveMunicipalityCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        var municipality = await _context.Municipalities.FirstOrDefaultAsync(x => x.Id == command.Id);

        if (municipality == default)
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.NotFound, "munícipio")));

        _context.Municipalities.Remove(municipality);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}