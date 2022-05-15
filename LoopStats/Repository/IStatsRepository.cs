using System.Threading;
using System.Threading.Tasks;

namespace LoopStats.Repository
{
    public interface IStatsRepository<T>
    {
        Task<T> Create(T entity, CancellationToken cancellationToken = default);
    }
}