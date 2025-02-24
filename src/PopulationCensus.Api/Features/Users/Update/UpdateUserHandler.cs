namespace PopulationCensus.Api.Features.Users.Update;

public record UpdateUserCommand(Guid Id, string UserName, string Email, string PhoneNumber, string Role)
    : ICommand<Result<bool>>;

public class UpdateCommandValidation : AbstractValidator<UpdateUserCommand>
{
    public UpdateCommandValidation()
    {
        RuleFor(x => x.Id)
           .NotNull().WithMessage(ErrorMessages.IdNotValid)
           .NotEqual(Guid.Empty).WithMessage(ErrorMessages.IdNotValid);

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "nome de utilizador"))
            .MinimumLength(5).WithMessage(String.Format(ErrorMessages.MinimumLength, "nome de utilizador", 5));

        RuleFor(x => x.Email)
           .NotEmpty().WithMessage(ErrorMessages.EmailNotValid);

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "perfil"));
    }
}

public class UpdateAdminHandler
    (UserManager<User> userManager,
    IValidator<UpdateUserCommand> validator)
    : ICommandHandler<UpdateUserCommand, Result<bool>>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IValidator<UpdateUserCommand> _validator = validator;

    public async Task<Result<bool>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        var user = await _userManager.FindByIdAsync(command.Id.ToString());

        if (user is null)
            return Result<bool>.Failure(new Error(string.Format(ErrorMessages.NotFound, "utilizador")));

        user.UserName = command.UserName;
        user.Email = command.Email;
        user.PhoneNumber = command.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => new Error(error.Description)).ToList();
            return Result<bool>.Failure(errors);
        }

        if (!String.IsNullOrWhiteSpace(command.Role))
        {
            var currentRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            await _userManager.RemoveFromRoleAsync(user, currentRole);

            await _userManager.AddToRoleAsync(user, command.Role);
        }

        return Result<bool>.Success(true);
    }
}