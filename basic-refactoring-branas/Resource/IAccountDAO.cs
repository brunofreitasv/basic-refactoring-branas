using basic_refactoring_branas.Application.Models;

namespace basic_refactoring_branas.Resource
{
    public interface IAccountDAO
    {
        void OpenConnection();
        void CloseConnection();
        Task<Account> GetAccontById(string accountId);
        Task<Account> GetAccontByEmail(string email);
        Task SaveAccount(Account account);
    }
}
