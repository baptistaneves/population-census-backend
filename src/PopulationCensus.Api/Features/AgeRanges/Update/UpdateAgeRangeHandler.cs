namespace PopulationCensus.Api.Features.Provinces.Update;

public record UpdateAgeRangeCommand(Guid Id, string Range, string Description) : ICommand<Result<bool>>;

public class UpdateAgeRangeCommandValidation : AbstractValidator<UpdateAgeRangeCommand>
{
    public UpdateAgeRangeCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage(ErrorMessages.IdNotValid)
            .NotEqual(Guid.Empty).WithMessage(ErrorMessages.IdNotValid);

        RuleFor(x => x.Range)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "faixa etária"))
            .MinimumLength(3).WithMessage(String.Format(ErrorMessages.MinimumLength, "faixa etária", 5));

        RuleFor(x => x.Description)
           .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "descrição"));
    }
}

public class UpdateAgeRangeHandler
    (IApplicationDbContext context,
     IValidator<UpdateAgeRangeCommand> validator)
    : ICommandHandler<UpdateAgeRangeCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<UpdateAgeRangeCommand> _validator = validator; 

    public async Task<Result<bool>> Handle(UpdateAgeRangeCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        var ageRange = await _context.AgeRanges.FirstOrDefaultAsync(x => x.Id == command.Id);

        if (ageRange == default)
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.NotFound, "faixa etária")));

        if (await _context.AgeRanges.AnyAsync(x => x.Range == command.Range && x.Id != command.Id, cancellationToken))
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.AlreadyExists, "faixa etária")));

        ageRange.Update(command.Range, command.Description);
        _context.AgeRanges.Update(ageRange);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}