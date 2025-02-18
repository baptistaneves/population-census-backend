namespace PopulationCensus.Api.Features.Provinces.Create;

public record CreateProvinceCommand(string Name) : ICommand<Result<bool>>;

public class CreateProvinceCommandValidation : AbstractValidator<CreateProvinceCommand>
{
    public CreateProvinceCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IdRequired, "nome"))
            .MinimumLength(3).WithMessage(String.Format(ErrorMessages.MinimumLength, "nome", 3));
    }
}

public class CreateProvinceHandler
    (IApplicationDbContext context,
    IValidator<CreateProvinceCommand> validator) : ICommandHandler<CreateProvinceCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<CreateProvinceCommand> _validator = validator;

    public async Task<Result<bool>> Handle(CreateProvinceCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        if (await _context.Provinces.AnyAsync(x => x.Name == command.Name, cancellationToken))
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.AlreadyExists, "província")));

        var newProvince = Province.Create(command.Name);

        _context.Provinces.Add(newProvince);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;

        return Result<bool>.Success(true);
    }
}