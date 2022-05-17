using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using LoopStats.Repository;
using LoopStats.Models.Entities;
using LoopStats.Services;
using AutoMapper;
using System.Collections.Generic;
using LoopStats.Models.DTOs;

namespace LoopStats.Functions
{
    public class GetHistoricalData
    {
        private readonly ILogger<GetHistoricalData> _log;
        private readonly IStatsRepository<LoopringStatsEntity> _statsRepository;
        private readonly ILoopringGraphConsumer _graphConsumer;
        private readonly IMapper _mapper;

        public GetHistoricalData(ILogger<GetHistoricalData> log, IStatsRepository<LoopringStatsEntity> statsRepository, ILoopringGraphConsumer graphConsumer,
            IMapper mapper)
        {
            _log = log;
            _statsRepository = statsRepository;
            _graphConsumer = graphConsumer;
            _mapper = mapper;
        }

        [FunctionName("GetHistoricalData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "historical/GetHistoricalData/{id:int}")] HttpRequest req, int id)
        {
            _log.LogInformation("C# HTTP trigger GetHistoricalData processed a request.");

            if (id < 1)
                return new BadRequestResult();

            var result = await _graphConsumer.GetHistoricalBlockStats(id);

            var stats = _mapper.Map<LoopringStatsEntity>(result);

            var lastBlock = await _statsRepository.GetByBlockId(id+1);

            stats.transactionCount = lastBlock.transactionCount - stats.transactionCount;
            stats.transferCount = lastBlock.transferCount - stats.transferCount;
            stats.transferNFTCount = lastBlock.transferNFTCount - stats.transferNFTCount;
            stats.tradeNFTCount = lastBlock.tradeNFTCount - stats.tradeNFTCount;
            stats.nftCount = lastBlock.nftCount - stats.nftMintCount;
            stats.nftMintCount = lastBlock.nftMintCount - stats.nftMintCount;

            stats.PartitionKey = "LoopyStatsQuarterly";
            string invertedTicks = string.Format("{0:D19}", DateTime.MaxValue.Ticks - stats.Timestamp.Ticks);
            stats.RowKey = invertedTicks;

            _log.LogInformation("Finished mapping, now sending to storage table");

            await _statsRepository.CreateAsync(stats);

            _log.LogInformation("Finished function.");

            return new OkObjectResult(stats);

        }

        [FunctionName("GetHistoricalDataMultiple")]
        public async Task<IActionResult> GetMultiple(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "historical/GetHistoricalDataMultiple/{from:int}/{to:int}")] HttpRequest req, int from, int to)
        {
            _log.LogInformation("C# HTTP trigger GetHistoricalDataMultiple processed a request.");

            if (from < 1 || to < 1)
                return new BadRequestResult();

            List<LoopringStatsEntity> result = new();

            for (int start = to; start >= from; start--)
            {
                var response = await _graphConsumer.GetHistoricalBlockStats(start);

                var stats = _mapper.Map<LoopringStatsEntity>(response);

                var lastBlock = await _statsRepository.GetByBlockId(start + 1);

                stats.transactionCount = lastBlock.transactionCount - stats.transactionCount;
                stats.transferCount = lastBlock.transferCount - stats.transferCount;
                stats.transferNFTCount = lastBlock.transferNFTCount - stats.transferNFTCount;
                stats.tradeNFTCount = lastBlock.tradeNFTCount - stats.tradeNFTCount;
                stats.nftCount = lastBlock.nftCount - stats.nftMintCount;
                stats.nftMintCount = lastBlock.nftMintCount - stats.nftMintCount;
                

                stats.PartitionKey = "LoopyStatsQuarterly";
                string invertedTicks = string.Format("{0:D19}", DateTime.MaxValue.Ticks - stats.Timestamp.Ticks);
                stats.RowKey = invertedTicks;

                _log.LogInformation("Finished mapping, now sending to storage table");

                await _statsRepository.CreateAsync(stats);


                result.Add(stats);
            }

            _log.LogInformation("Finished function.");

            return new OkObjectResult(result);
        }
    }
}
