namespace PopulationCensus.Api.Features.Provinces.Update;

public record UpdateProvinceCommand(Guid Id, string Name) : ICommand<Result<bool>>;

public class UpdateProvinceCommandValidation : AbstractValidator<UpdateProvinceCommand>
{
    public UpdateProvinceCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage(ErrorMessages.IdNotValid)
            .NotEqual(Guid.Empty).WithMessage(ErrorMessages.IdNotValid);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "nome"))
            .MinimumLength(3).WithMessage(String.Format(ErrorMessages.MinimumLength, "nome", 3));
    }
}

public class UpdateProvinceHandler
    (IApplicationDbContext context,
     IValidator<UpdateProvinceCommand> validator)
    : ICommandHandler<UpdateProvinceCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<UpdateProvinceCommand> _validator = validator; 

    public async Task<Result<bool>> Handle(UpdateProvinceCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        var province = await _context.Provinces.SingleOrDefaultAsync(x => x.Id == command.Id);

        if (province is null)
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.NotFound, "província")));

        if (await _context.Provinces.AnyAsync(x => x.Name == command.Name && x.Id != command.Id, cancellationToken))
            return Result<bool>.Failure(new Error(String.Format(ErrorMessages.AlreadyExists, "província")));

        province.Update(command.Name);
        _context.Provinces.Update(province);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}