using basic_refactoring_branas.Application.Models;

namespace basic_refactoring_branas.Application
{
    public interface IAccountService
    {
        Task<SignupResult> SignupAsync(Account input);
        Task<Account> GetAccontAsync(string accountId);
    }
}
