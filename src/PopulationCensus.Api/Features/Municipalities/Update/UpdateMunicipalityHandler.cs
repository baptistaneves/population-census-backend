namespace PopulationCensus.Api.Features.Provinces.Update;

public record UpdateMunicipalityCommand(Guid Id, string Name, Guid ProvinceId) : ICommand<Result<bool>>;

public class UpdateMunicipalityCommandValidation : AbstractValidator<UpdateMunicipalityCommand>
{
    public UpdateMunicipalityCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage(ErrorMessages.IdNotValid)
            .NotEqual(Guid.Empty).WithMessage(ErrorMessages.IdNotValid);

        RuleFor(x => x.ProvinceId)
           .NotNull().WithMessage(String.Format(ErrorMessages.IsRequired, "província"))
           .NotEqual(Guid.Empty).WithMessage(String.Format(ErrorMessages.IsRequired, "província"));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "nome"))
            .MinimumLength(3).WithMessage(String.Format(ErrorMessages.MinimumLength, "nome", 3));
    }
}

public class UpdateMunicipalityHandler
    (IApplicationDbContext context,
     IValidator<UpdateMunicipalityCommand> validator)
    : ICommandHandler<UpdateMunicipalityCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<UpdateMunicipalityCommand> _validator = validator; 

    public async Task<Result<bool>> Handle(UpdateMunicipalityCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var municipality = await _context.Municipalities.FirstOrDefaultAsync(x => x.Id == command.Id);

        if (municipality == default)
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.NotFound, "munícipio")));

        if (await _context.Municipalities.AnyAsync(x => x.Name == command.Name && x.Id != command.Id, cancellationToken))
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.AlreadyExists, "munícipio")));

        municipality.Update(command.Name, command.ProvinceId);
        _context.Municipalities.Update(municipality);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}