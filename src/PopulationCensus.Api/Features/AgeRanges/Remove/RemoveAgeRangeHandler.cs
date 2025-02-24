namespace PopulationCensus.Api.Features.Provinces.Remove;

public record RemoveAgeRangeCommand(Guid Id) : ICommand<Result<bool>>;

public class RemoveAgeRangeCommandValidation : AbstractValidator<RemoveAgeRangeCommand>
{
    public RemoveAgeRangeCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage(ErrorMessages.IdNotValid)
            .NotEqual(Guid.Empty).WithMessage(ErrorMessages.IdNotValid);
    }
}

public class RemoveAgeRangeHandler
    (IApplicationDbContext context,
     IValidator<RemoveAgeRangeCommand> validator)
    : ICommandHandler<RemoveAgeRangeCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<RemoveAgeRangeCommand> _validator = validator;

    public async Task<Result<bool>> Handle(RemoveAgeRangeCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        var ageRange = await _context.AgeRanges.FirstOrDefaultAsync(x => x.Id == command.Id);

        if (ageRange == default)
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.NotFound, "faixa etária")));

        _context.AgeRanges.Remove(ageRange);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}