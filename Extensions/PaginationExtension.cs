using CP.Api.DTOs.Response;

namespace CP.Api.Extensions;

public static class PaginationExtension
{
    /// <summary>
    ///     Get the entities from the clustered pagination of the list
    /// </summary>
    /// <param name="source">the list of entities</param>
    /// <param name="parameter">the page parameter</param>
    /// <typeparam name="TEntity">the entity</typeparam>
    /// <returns>the list of entities from the paginated list</returns>
    public static PaginatedEnumerable<TEntity> GetPage<TEntity>(this IEnumerable<TEntity> source,
        PaginationParameter parameter)
    {
        IEnumerable<TEntity> enumerable = source.ToList();
        IEnumerable<TEntity> items = enumerable.Skip((parameter.PageNumber - 1) * parameter.PageSize)
            .Take(parameter.PageSize);
        int totalRecord = enumerable.Count();
        int totalPage = (int)Math.Ceiling(totalRecord / (double)parameter.PageSize);
        bool hasNextPage = parameter.PageNumber < totalPage;
        bool hasPreviousPage = parameter.PageNumber > 1;
        return new PaginatedEnumerable<TEntity>
        {
            Items = items,
            TotalRecord = totalRecord,
            TotalPage = totalPage,
            HasNextPage = hasNextPage,
            HasPreviousPage = hasPreviousPage,
            PageNumber = parameter.PageNumber,
            PageSize = parameter.PageSize
        };
    }

    /// <summary>
    ///     Get the entities from the clustered pagination of the list
    /// </summary>
    /// <param name="source">the list of entities</param>
    /// <param name="parameter">the page parameter</param>
    /// <typeparam name="TEntity">the entity</typeparam>
    /// <returns>the list of entities from the paginated list</returns>
    public static PaginatedEnumerable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> source,
        PaginationParameter parameter)
    {
        IQueryable<TEntity> items = source.Skip((parameter.PageNumber - 1) * parameter.PageSize)
            .Take(parameter.PageSize);
        int totalRecord = source.Count();
        int totalPage = (int)Math.Ceiling(totalRecord / (double)parameter.PageSize);
        bool hasNextPage = parameter.PageNumber < totalPage;
        bool hasPreviousPage = parameter.PageNumber > 1;
        return new PaginatedEnumerable<TEntity>
        {
            Items = items,
            TotalRecord = totalRecord,
            TotalPage = totalPage,
            HasNextPage = hasNextPage,
            HasPreviousPage = hasPreviousPage,
            PageNumber = parameter.PageNumber,
            PageSize = parameter.PageSize
        };
    }
}