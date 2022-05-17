using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using LoopStats.Models.Entities;
using LoopStats.Repository;
using LoopStats.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace LoopStats
{
    public class LoopringStats
    {
        private readonly ILoopringGraphConsumer _graphConsumer;
        private readonly Repository.IStatsRepository<LoopringStatsEntity> _statsRepository;
        private readonly IMapper _mapper;

        public LoopringStats(ILoopringGraphConsumer graphConsumer, IStatsRepository<LoopringStatsEntity> statsRepository, IMapper mapper)
        {
            _graphConsumer = graphConsumer;
            _statsRepository = statsRepository;
            _mapper = mapper;
        }

        [FunctionName("LoopringQuarterlyStats")]
        public async Task Quarterly([TimerTrigger("0 */15 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var result = await _graphConsumer.GetBlockStats();

            var stats = _mapper.Map<LoopringStatsEntity>(result);

            // Check to see if the block data already exist
            var existingBlock = _statsRepository.GetByBlockId(stats.blockCount);

            if (existingBlock != null)
                await Task.CompletedTask;

            stats.PartitionKey = "LoopyStatsQuarterly";
            string invertedTicks = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
            stats.RowKey = invertedTicks;

            log.LogInformation("Finished mapping, now sending to storage table");

            await _statsRepository.CreateAsync(stats);

            log.LogInformation("Finished function.");
        }

        [FunctionName("LoopringDailyStats")]
        public async Task Daily([TimerTrigger("0 30 0 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger Daily executed at: {DateTime.Now}");

            var result = await _graphConsumer.GetBlockStats();

            var stats = _mapper.Map<LoopringStatsEntity>(result);

            stats.PartitionKey = "LoopyStatsDaily";
            string invertedTicks = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
            stats.RowKey = invertedTicks;

            log.LogInformation("Finished mapping, now sending to storage table");

            await _statsRepository.CreateAsync(stats);

            log.LogInformation("Finished function.");
        }
    }
}
