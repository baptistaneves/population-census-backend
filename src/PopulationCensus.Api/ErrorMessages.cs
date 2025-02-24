namespace PopulationCensus.Api;

public static class ErrorMessages
{
    public const string IdNotValid = "O ID informado não é valido";
    public const string NotFound = "Nenhum(a) {0} foi encontrada(o) com o ID solicitado";
    public const string AlreadyExists = "Já existe um(a) {0} cadastrado(a) com este nome/descrição";
    public const string IsRequired = "O campo {0} deve ser informado";
    public const string EmailNotValid = "O e-mail não é válido";
    public const string MinimumLength = "O campo {0} deve ter no minímo {1} caracteres";
    public const string MaxLength = "O campo {0} deve ter no máximo {1} caracteres";
    
    public const string WrongUserNameOrPassword = "Nome de utilizador ou senha errada";
    public const string LockedOutUser = "Usuário temporariamente bloqueado";


    public const string ProvinceCanNotBeRemoved = "Esta província possui munícipios cadastrados, não pode removida.";
}