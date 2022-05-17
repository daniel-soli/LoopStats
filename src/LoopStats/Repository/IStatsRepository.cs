using LoopStats.Models.DTOs;
using LoopStats.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace LoopStats.Repository
{
    public interface IStatsRepository<T>
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<AllStatsDto> GetAllAsync(CancellationToken cancellationToken = default);
        Task<AllStatsDto> GetAllDailyStatsAsync();
        Task<LastDayStatsDto> GetLastDaysStatsAsync(CancellationToken cancellationToken = default);
        Task<LoopringStatsEntity> GetLatestStatAsync(CancellationToken cancellationToken = default);
    }
}