namespace PopulationCensus.Api.Features.Login;

public record LoginResult(Guid Id, string Token, string Email, string UserName);

public record LoginCommand(string Email, string Password) : ICommand<Result<LoginResult>>;

public class LoginHandler
    (SignInManager<User> signInManager,
     UserManager<User> userManager,
     IJwtService jwtService)
    : ICommandHandler<LoginCommand, Result<LoginResult>>
{
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IJwtService _jwtService = jwtService;

    public async Task<Result<LoginResult>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);

        if (user is null)
            return Result<LoginResult>.Failure(new Error(ErrorMessages.WrongUserNameOrPassword));

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, command.Password, true);

        if (signInResult.IsLockedOut)
            return Result<LoginResult>.Failure(new Error(ErrorMessages.LockedOutUser));

        if (!signInResult.Succeeded)
            return Result<LoginResult>.Failure(new Error(ErrorMessages.WrongUserNameOrPassword));

        return Result<LoginResult>.Success(new LoginResult
        (
            user.Id,
            await _jwtService.GetJwtString(user),
            user.Email,
            user.UserName
        ));
    }

}