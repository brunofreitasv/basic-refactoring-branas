namespace basic_refactoring_branas.Application.Models
{
    public sealed class SignupResult
    {
        public static readonly SignupResult AlreadyExists = new SignupResult(-4, "Account already exists!");
        public static readonly SignupResult InvalidName = new SignupResult(-3, "Invalid name!");
        public static readonly SignupResult InvalidEmail = new SignupResult(-2, "Invalid email!");
        public static readonly SignupResult InvalidCpf = new SignupResult(-1, "Invalid cpf!");
        public static readonly SignupResult InvalidCarPlate = new SignupResult(-5, "Invalid car plate!");

        public int Code { get; private set; }
        public string Message { get; private set; }
        public string AccountId { get; private set; }
        public bool IsSuccessCode => Code == 0;

        public SignupResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public SignupResult(string accountId) : this(0, "Account created successfully!")
        {
            AccountId = accountId;
        }
    }
}
