using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoopStats.Models.DTOs;

namespace LoopStats.Common.Mappings;

public static class MappingExtensions
{
    public static PaginatedList<TDestination> PaginatedList<TDestination>(this IEnumerable<TDestination> queryable, int pageIndex, int pageSize)
        => Models.DTOs.PaginatedList<TDestination>.Create(queryable, pageIndex, pageSize);
    
    public static async Task<PaginatedList<TDestination>> PaginatedList<TDestination>(this IQueryable<TDestination> queryable, int pageIndex, int pageSize)
        => await Models.DTOs.PaginatedList<TDestination>.CreateAsync(queryable, pageIndex, pageSize);

    public static async Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
        => await queryable.ProjectTo<TDestination>(configuration).ToListAsync<TDestination>();
}
