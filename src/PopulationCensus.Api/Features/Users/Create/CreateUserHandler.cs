namespace PopulationCensus.Api.Features.Users.Create;

public record CreateUserCommand(string Email, string UserName, string PhoneNumber, string Role, string Password)
    : ICommand<Result<bool>>;

public class CreatUserCommandValidation : AbstractValidator<CreateUserCommand>
{
    public CreatUserCommandValidation()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "nome de utilizador"))
            .MinimumLength(5).WithMessage(String.Format(ErrorMessages.MinimumLength, "nome de utilizador", 5));

        RuleFor(x => x.Email)
           .NotEmpty().WithMessage(ErrorMessages.EmailNotValid);

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "perfil"));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(String.Format(ErrorMessages.IsRequired, "senha"));
    }
}

public class CreateUserHandler
    (UserManager<User> userManager,
     IValidator<CreateUserCommand> validator)
    : ICommandHandler<CreateUserCommand, Result<bool>>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IValidator<CreateUserCommand> _validator = validator;


    public async Task<Result<bool>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await CommandValidator.ValidateAsync(command, _validator);

        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors!);

        var newUser = new User
        {
            UserName = command.UserName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        };

        var userResult = await _userManager.CreateAsync(newUser, command.Password);

        if (!userResult.Succeeded)
        {
            var errors = userResult.Errors.Select(error => new Error(error.Description)).ToList();
            return Result<bool>.Failure(errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(newUser, command.Role);

        if (!roleResult.Succeeded)
        {
            var errors = roleResult.Errors.Select(error => new Error(error.Description)).ToList();
            return Result<bool>.Failure(errors);
        }

        return Result<bool>.Success(true);
    }

}