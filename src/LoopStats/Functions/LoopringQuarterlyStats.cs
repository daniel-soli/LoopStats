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
    public class LoopringQuarterlyStats
    {
        private readonly ILoopringGraphConsumer _graphConsumer;
        private readonly Repository.IStatsRepository<LoopringStatsEntity> _statsRepository;
        private readonly IMapper _mapper;

        public LoopringQuarterlyStats(ILoopringGraphConsumer graphConsumer, IStatsRepository<LoopringStatsEntity> statsRepository, IMapper mapper)
        {
            _graphConsumer = graphConsumer;
            _statsRepository = statsRepository;
            _mapper = mapper;
        }

        [FunctionName("LoopringQuarterlyStats")]
        public async Task Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var result = await _graphConsumer.GetBlockStats();

            var stats = _mapper.Map<LoopringStatsEntity>(result);

            stats.PartitionKey = "LoopyStats";
            string invertedTicks = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
            stats.RowKey = invertedTicks;

            log.LogInformation("Finished mapping, now sending to storage table");

            await _statsRepository.CreateAsync(stats);

            log.LogInformation("Finished function.");
        }
    }
}
