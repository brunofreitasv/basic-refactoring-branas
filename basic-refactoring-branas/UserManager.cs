using System.Text.RegularExpressions;
using basic_refactoring_branas.Dapper;

namespace basic_refactoring_branas
{
    public class UserManager
    {
        private readonly IDatabase _database;

        public UserManager(IDatabase database)
        {
            _database = database;
        }

        public async Task<SignupResult> SignupAsync(Account input)
        {
            try
            {
                _database.Connection.Open();
                return await SaveAccountAsync(input);
            }
            finally
            {
                _database.Connection.Close();
            }
        }

        private async Task<SignupResult> SaveAccountAsync(Account input)
        {
            string id = Guid.NewGuid().ToString();

            var existingAccount = await _database.QueryFirstOrDefaultAsync<Account>("select * from cccat17.account where email = @Email", new { Email = input.Email });

            if (existingAccount != null)
            {
                return SignupResult.AlreadyExists;
            }

            if (!ValidateName(input.Name))
            {
                return SignupResult.InvalidName;
            }

            if (!ValidateEmail(input.Email))
            {
                return SignupResult.InvalidEmail;
            }

            if (!CpfValidator.ValidateCpf(input.Cpf))
            {
                return SignupResult.InvalidCpf;
            }

            if (input.IsDriver)
            {
                if (!ValidateCarPlate(input.CarPlate))
                {
                    return SignupResult.InvalidCarPlate;
                }
            }

            await _database.ExecuteAsync("insert into cccat17.account (account_id, name, email, cpf, car_plate, is_passenger, is_driver) values (@AccountId, @Name, @Email, @Cpf, @CarPlate, @IsPassenger, @IsDriver)",
                new
                {
                    AccountId = id,
                    Name = input.Name,
                    Email = input.Email,
                    Cpf = input.Cpf,
                    CarPlate = input.CarPlate,
                    IsPassenger = input.IsPassenger,
                    IsDriver = input.IsDriver
                });

            return new SignupResult(id);
        }

        private bool ValidateName(string name) => name.Split(' ').Length == 2;

        private bool ValidateEmail(string email) => Regex.IsMatch(email, @"^(.+)@(.+)$");

        private bool ValidateCarPlate(string carPlate) => Regex.IsMatch(carPlate, @"^[A-Z]{3}[0-9]{4}$");
    }
}
