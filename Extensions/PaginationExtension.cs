﻿using CP.Api.DTOs.Response;

namespace CP.Api.Extensions;

public static class PaginationExtension
{
    /// <summary>
    /// Get the entities from the clustered pagination of the list
    /// </summary>
    /// <param name="entities">the list of entities</param>
    /// <param name="pageSize">the size of the page</param>
    /// <param name="pageNumber">the page number to get the entities, start from 1</param>
    /// <typeparam name="TEntity">the entity</typeparam>
    /// <returns>the list of entities from the paginated list</returns>
    public static IEnumerable<TEntity> GetPage<TEntity>(this IEnumerable<TEntity> entities, int pageSize, int pageNumber)
    {
        return entities
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    /// <summary>
    /// Get the entities from the clustered pagination of the list
    /// </summary>
    /// <param name="entities">the list of entities</param>
    /// <param name="parameter">the page parameter</param>
    /// <typeparam name="TEntity">the entity</typeparam>
    /// <returns>the list of entities from the paginated list</returns>
    public static IEnumerable<TEntity> GetPage<TEntity>(this IEnumerable<TEntity> entities, PaginationParameter parameter)
    {
        return GetPage<TEntity>(entities, parameter.PageSize, parameter.PageNumber);
    }

    /// <summary>
    /// Count the list but we assign it to an out parameter
    /// </summary>
    /// <param name="entities">the list of entities</param>
    /// <param name="count">the amount of entities in the list</param>
    /// <typeparam name="TEntity">the entity</typeparam>
    /// <returns>the list</returns>
    public static IEnumerable<TEntity> GetCount<TEntity>(this IEnumerable<TEntity> entities, out int count)
    {
        count = entities.Count();
        return entities;
    }

    /// <summary>
    /// Get the entities from the clustered pagination of the list
    /// </summary>
    /// <param name="entities">the list of entities</param>
    /// <param name="pageSize">the size of the page</param>
    /// <param name="pageNumber">the page number to get the entities, start from 1</param>
    /// <typeparam name="TEntity">the entity</typeparam>
    /// <returns>the list of entities from the paginated list</returns>
    public static IQueryable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> entities, int pageSize, int pageNumber)
    {
        return entities
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    /// <summary>
    /// Get the entities from the clustered pagination of the list
    /// </summary>
    /// <param name="entities">the list of entities</param>
    /// <param name="parameter">the page parameter</param>
    /// <typeparam name="TEntity">the entity</typeparam>
    /// <returns>the list of entities from the paginated list</returns>
    public static IQueryable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> entities, PaginationParameter parameter)
    {
        return GetPage<TEntity>(entities, parameter.PageSize, parameter.PageNumber);
    }

    /// <summary>
    /// Count the list but we assign it to an out parameter
    /// </summary>
    /// <param name="entities">the list of entities</param>
    /// <param name="count">the amount of entities in the list</param>
    /// <typeparam name="TEntity">the entity</typeparam>
    /// <returns>the list</returns>
    public static IQueryable<TEntity> GetCount<TEntity>(this IQueryable<TEntity> entities, out int count)
    {
        count = entities.Count();
        return entities;
    }
}