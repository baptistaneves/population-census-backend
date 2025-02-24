namespace PopulationCensus.Api.Features.Users.Remove;

public record RemoveUserCommand(Guid Id) : ICommand<Result<bool>>;

public class RemoveUserCommandValidation : AbstractValidator<RemoveUserCommand>
{
    public RemoveUserCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage(ErrorMessages.IdNotValid)
            .NotEqual(Guid.Empty).WithMessage(ErrorMessages.IdNotValid);
    }
}

public class RemoveAdminHandler
    (UserManager<User> userManager,
     IValidator<RemoveUserCommand> validator)
    : ICommandHandler<RemoveUserCommand, Result<bool>>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IValidator<RemoveUserCommand> _validator = validator;

    public async Task<Result<bool>> Handle(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        var user = await userManager.FindByIdAsync(command.Id.ToString());

        if (user is null)
            return Result<bool>.Failure(new Error(string.Format(ErrorMessages.NotFound, "utilizador")));

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => new Error(error.Description)).ToList();
            return Result<bool>.Failure(errors);
        }

        return Result<bool>.Success(true);
    }

}