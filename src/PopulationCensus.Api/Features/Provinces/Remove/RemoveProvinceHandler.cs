namespace PopulationCensus.Api.Features.Provinces.Remove;

public record RemoveProvinceCommand(Guid Id) : ICommand<Result<bool>>;

public class RemoveProvinceCommandValidation : AbstractValidator<RemoveProvinceCommand>
{
    public RemoveProvinceCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("O Id do campo deve ser informado")
            .NotEqual(Guid.Empty).WithMessage("O Id do campo deve ser informado");
    }
}

public class RemoveProvinceHandler
    (IApplicationDbContext context,
     IValidator<RemoveProvinceCommand> validator)
    : ICommandHandler<RemoveProvinceCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<RemoveProvinceCommand> _validator = validator;

    public async Task<Result<bool>> Handle(RemoveProvinceCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        var province = await _context.Provinces.SingleOrDefaultAsync(x => x.Id == command.Id);

        if (province is null)
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.NotFound, "província")));

        _context.Provinces.Remove(province);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}