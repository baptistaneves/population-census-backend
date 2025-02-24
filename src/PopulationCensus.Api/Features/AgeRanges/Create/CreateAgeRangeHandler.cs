namespace PopulationCensus.Api.Features.Provinces.Create;

public record CreateAgeRangeCommand(string Range, string Description) : ICommand<Result<bool>>;

public class CreateAgeRangeCommandValidation : AbstractValidator<CreateAgeRangeCommand>
{
    public CreateAgeRangeCommandValidation()
    {
        RuleFor(x => x.Range)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "faixa etária"))
            .MinimumLength(3).WithMessage(String.Format(ErrorMessages.MinimumLength, "faixa etária", 5));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "descrição"));
    }
}

public class CreateAgeRangeHandler
    (IApplicationDbContext context,
    IValidator<CreateAgeRangeCommand> validator) : ICommandHandler<CreateAgeRangeCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<CreateAgeRangeCommand> _validator = validator;

    public async Task<Result<bool>> Handle(CreateAgeRangeCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        if (await _context.AgeRanges.AnyAsync(x => x.Range == command.Range, cancellationToken))
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.AlreadyExists, "faixa etária")));

        var newAgeRange = AgeRange.Create(command.Range, command.Description);

        _context.AgeRanges.Add(newAgeRange);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;

        return Result<bool>.Success(true);
    }
}