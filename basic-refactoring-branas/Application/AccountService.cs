using System.Text.RegularExpressions;
using basic_refactoring_branas.Application.Models;
using basic_refactoring_branas.Application.Utils;
using basic_refactoring_branas.Resource;

namespace basic_refactoring_branas.Application
{
    public class AccountService (IAccountDAO _accountDao) : IAccountService
    {
        public async Task<SignupResult> SignupAsync(Account input)
        {
            try
            {
                _accountDao.OpenConnection();
                return await SaveAccountAsync(input);
            }
            finally
            {
                _accountDao.CloseConnection();
            }
        }

        public Task<Account> GetAccontAsync(string accountId)
        {
            return _accountDao.GetAccontById(accountId);
        }

        private async Task<SignupResult> SaveAccountAsync(Account input)
        {
            string id = Guid.NewGuid().ToString();

            var existingAccount = await _accountDao.GetAccontByEmail(input.Email);

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

            if (input.IsDriver.HasValue && input.IsDriver.Value)
            {
                if (!ValidateCarPlate(input.CarPlate))
                {
                    return SignupResult.InvalidCarPlate;
                }
            }

            input.AccountId = id;
            await _accountDao.SaveAccount(input);
            
            return new SignupResult(id);
        }

        private bool ValidateName(string name) => !string.IsNullOrEmpty(name) && name.Split(' ').Length == 2;

        private bool ValidateEmail(string email) => !string.IsNullOrEmpty(email) && Regex.IsMatch(email, @"^(.+)@(.+)$");

        private bool ValidateCarPlate(string carPlate) => !string.IsNullOrEmpty(carPlate) && Regex.IsMatch(carPlate, @"^[A-Z]{3}[0-9]{4}$");
    }
}
