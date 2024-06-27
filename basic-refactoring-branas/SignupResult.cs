namespace basic_refactoring_branas
{
    public sealed class SignupResult
    {
        public static readonly SignupResult AlreadyExists = new SignupResult(-4);
        public static readonly SignupResult InvalidName = new SignupResult(-3);
        public static readonly SignupResult InvalidEmail = new SignupResult(-2);
        public static readonly SignupResult InvalidCpf = new SignupResult(-1);
        public static readonly SignupResult InvalidCarPlate = new SignupResult(-5);

        public int Code { get; private set; }
        public string AccountId { get; private set; }

        public SignupResult(int code)
        {
            Code = code;
        }

        public SignupResult(string accountId) : this(0)
        {
            AccountId = accountId;
        }
    }
}
