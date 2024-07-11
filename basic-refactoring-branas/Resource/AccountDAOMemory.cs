using basic_refactoring_branas.Application.Models;

namespace basic_refactoring_branas.Resource
{
    public class AccountDAOMemory : IAccountDAO
    {
        private readonly List<Account> _accounts;

        public AccountDAOMemory()
        {
            _accounts = new List<Account>();
        }

        public void OpenConnection()
        {
            return;
        }

        public void CloseConnection()
        {
            return;
        }

        public Task<Account> GetAccontByEmail(string email)
        {
            var account = _accounts.Find(a => a.Email.Equals(email));
            return Task.FromResult(account);
        }

        public Task<Account> GetAccontById(string accountId)
        {
            var account = _accounts.Find(a => a.AccountId.Equals(accountId));
            return Task.FromResult(account);
        }

        public Task SaveAccount(Account account)
        {
            _accounts.Add(account);
            return Task.CompletedTask;
        }
    }
}
