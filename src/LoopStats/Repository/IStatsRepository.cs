using LoopStats.Models.DTOs;
using LoopStats.Models.Entities;
using LoopStats.Models.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace LoopStats.Repository
{
    public interface IStatsRepository<T>
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<AllStatsDto> GetAllAsync(CancellationToken cancellationToken = default);
        Task<PaginatedList<StatsDto>> GetBlocksQuery(GetBlockStatsQuery query, CancellationToken cancellationToken = default);
        Task<AllStatsDto> GetAllDailyStatsAsync(CancellationToken cancellationToken = default);
        Task<LoopringStatsEntity> GetByBlockId(long blockId, CancellationToken cancellationToken = default);
        Task<LastDayStatsDto> GetCountFromToday(CancellationToken cancellationToken = default);
        Task<LastDayStatsDto> GetLastDaysStatsAsync(CancellationToken cancellationToken = default);
        Task<LoopringStatsEntity> GetLatestStatAsync(CancellationToken cancellationToken = default);
    }
}