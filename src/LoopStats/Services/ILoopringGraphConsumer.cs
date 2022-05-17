using LoopStats.Models.DTOs;
using System.Threading.Tasks;

namespace LoopStats.Services
{
    public interface ILoopringGraphConsumer
    {
        Task<BlockStatsDto> GetBlockStats();
        Task<SingleBlockStatsDto> GetHistoricalBlockStats(int blockId);
    }
}