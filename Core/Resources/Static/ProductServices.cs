using CP.Api.Services;

namespace CP.Api.Core.Resources.Static;

public static class ProductServices
{
    private static List<ServiceDescriptor> s_services = new();

    public static IEnumerable<ServiceDescriptor> Services =>
        s_services.Count switch
        {
            > 0 => s_services,
            _ => s_services = new List<ServiceDescriptor>
            {
                new ServiceDescriptor(
                    typeof(IAccountService),
                    typeof(AccountService),
                    ServiceLifetime.Scoped
                ),
                new ServiceDescriptor(
                    typeof(IVoteService),
                    typeof(VoteService),
                    ServiceLifetime.Scoped
                ),
                new ServiceDescriptor(
                    typeof(ICategoryService),
                    typeof(CategoryService),
                    ServiceLifetime.Scoped
                ),
                new ServiceDescriptor(
                    typeof(ICommentService),
                    typeof(CommentService),
                    ServiceLifetime.Scoped
                ),
                new ServiceDescriptor(
                    typeof(IStatisticService),
                    typeof(StatisticService),
                    ServiceLifetime.Scoped
                )
            }
        };
}