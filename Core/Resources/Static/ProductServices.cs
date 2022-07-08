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
                new(
                    typeof(IAccountService),
                    typeof(AccountService),
                    ServiceLifetime.Scoped
                ),
                new(
                    typeof(IVoteService),
                    typeof(VoteService),
                    ServiceLifetime.Scoped
                ),
                new(
                    typeof(ICategoryService),
                    typeof(CategoryService),
                    ServiceLifetime.Scoped
                ),
                new(
                    typeof(ICommentService),
                    typeof(CommentService),
                    ServiceLifetime.Scoped
                ),
                new(
                    typeof(IStatisticService),
                    typeof(StatisticService),
                    ServiceLifetime.Scoped
                )
            }
        };
}