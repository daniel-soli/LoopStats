using LoopStats.DTOs;
using System.Threading.Tasks;

namespace LoopStats.Services
{
    public interface ILoopringGraphConsumer
    {
        Task<BlockStatsDto> GetBlockStats();
    }
}