using basic_refactoring_branas.Application;
using basic_refactoring_branas.Resource;

namespace basic_refactoring_branas.API
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddSingleton<IAccountDAO, AccountDAOMemory>();
        }
    }
}
