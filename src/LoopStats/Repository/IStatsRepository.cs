using LoopStats.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace LoopStats.Repository
{
    public interface IStatsRepository<T>
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<LoopringStatsEntity> GetLatestStatAsync(CancellationToken cancellationToken = default);
    }
}