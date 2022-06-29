using CP.Api.Services;

namespace CP.Api.Core.Resources.Static;

public static class ProductServices
{
    static List<ServiceDescriptor> s_services = new();

    public static List<ServiceDescriptor> Services
    {
        get => s_services.Count switch
        {
            > 0 => s_services,
            _ => s_services = new()
                {
                    new
                    (
                        serviceType: typeof(IAccountService),
                        implementationType: typeof(AccountService),
                        lifetime: ServiceLifetime.Scoped
                    ),
                    new
                    (
                        serviceType: typeof(IVoteService),
                        implementationType: typeof(VoteService),
                        lifetime: ServiceLifetime.Scoped
                    ),
                    new
                    (
                        serviceType: typeof(ICategoryService),
                        implementationType: typeof(CategoryService),
                        lifetime: ServiceLifetime.Scoped
                    ),
                }
        };
    }
}
