using Ardalis.ApiEndpoints;
using basic_refactoring_branas.Application;
using basic_refactoring_branas.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace basic_refactoring_branas.API.Endpoints.Accounts
{
    public class GetAccount(IAccountService accountService) : EndpointBaseAsync
        .WithRequest<string>
        .WithActionResult<Account>

    {
        [HttpGet("accounts/{accountId}")]
        public override async Task<ActionResult<Account>> HandleAsync(string accountId, CancellationToken cancellationToken = default)
        {
            var account = await accountService.GetAccontAsync(accountId);

            if (account == null)
                return NotFound();

            return Ok(account);
        }
    }
}
