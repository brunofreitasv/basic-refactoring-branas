using Ardalis.ApiEndpoints;
using basic_refactoring_branas.Application;
using basic_refactoring_branas.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace basic_refactoring_branas.API.Endpoints.Accounts
{
    public class PostSignup(IAccountService accountService) : EndpointBaseAsync
        .WithRequest<Account>
        .WithActionResult<string>

    {
        [HttpPost("signup")]
        public override async Task<ActionResult<string>> HandleAsync(Account account, CancellationToken cancellationToken = default)
        {
            var result = await accountService.SignupAsync(account);

            if (!result.IsSuccessCode)
                return UnprocessableEntity(result.Message);

            return Ok(result.AccountId);
        }
    }
}
