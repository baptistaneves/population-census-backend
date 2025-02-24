namespace PopulationCensus.Api.Features.Provinces.Create;

public record CreateMunicipalityCommand(string Name, Guid ProvinceId) : ICommand<Result<bool>>;

public class CreateMuniciplaityCommandValidation : AbstractValidator<CreateMunicipalityCommand>
{
    public CreateMuniciplaityCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "nome"))
            .MinimumLength(3).WithMessage(String.Format(ErrorMessages.MinimumLength, "nome", 3));

        RuleFor(x => x.ProvinceId)
            .NotEqual(Guid.Empty).WithMessage(String.Format(ErrorMessages.IsRequired, "província"));
    }
}

public class CreateMunicipalityHandler
    (IApplicationDbContext context,
    IValidator<CreateMunicipalityCommand> validator) : ICommandHandler<CreateMunicipalityCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<CreateMunicipalityCommand> _validator = validator;

    public async Task<Result<bool>> Handle(CreateMunicipalityCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        if (await _context.Municipalities.AnyAsync(x => x.Name == command.Name, cancellationToken))
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.AlreadyExists, "munícipio")));

        var newMunicipality = Municipality.Create(command.Name, command.ProvinceId);

        _context.Municipalities.Add(newMunicipality);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;

        return Result<bool>.Success(true);
    }
}