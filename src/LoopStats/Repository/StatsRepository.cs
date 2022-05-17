using AutoMapper;
using LoopStats.Models.DTOs;
using LoopStats.Models.Entities;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoopStats.Repository;

public class StatsRepository<T> : IStatsRepository<T> where T : class, ITableEntity
{
    private readonly CloudTable _table;
    private readonly IMapper _mapper;

    public StatsRepository(CloudTable table, IMapper mapper)
    {
        _table = table;
        _mapper = mapper;
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            var mergeOperation = TableOperation.InsertOrMerge(entity);
            var result = (await _table.ExecuteAsync(mergeOperation, cancellationToken)).Result as T;

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<LoopringStatsEntity> GetLatestStatAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "LoopyStatsQuarterly");
            TableQuery<LoopringStatsEntity> query = new TableQuery<LoopringStatsEntity>().Where(filter);
            TableContinuationToken token = null;
            var response = await _table.ExecuteQuerySegmentedAsync(query, token);

            var result = response.FirstOrDefault();

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<LastDayStatsDto> GetLastDaysStatsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "LoopyStatsQuarterly");
            TableQuery<LoopringStatsEntity> query = new TableQuery<LoopringStatsEntity>().Where(filter).Take(144);
            TableContinuationToken token = null;
            var response = await _table.ExecuteQuerySegmentedAsync(query, token);

            var latest = response.FirstOrDefault();
            var oldest = response.LastOrDefault();

            LastDayStatsDto result = new LastDayStatsDto()
            {
                blockCount = latest.blockCount - oldest.blockCount,
                nftCount = latest.nftCount - oldest.nftCount,
                nftMintCount = latest.nftMintCount - oldest.nftMintCount,
                tradeNFTCount = latest.tradeNFTCount - oldest.tradeNFTCount,
                transactionCount = latest.transactionCount - oldest.transactionCount,
                transferCount = latest.transferCount - oldest.transferCount,
                transferNFTCount = latest.transferNFTCount - oldest.transferNFTCount,
                userCount = latest.userCount - oldest.userCount
            };

            return result;

        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<AllStatsDto> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "LoopyStatsQuarterly");
            TableQuery<LoopringStatsEntity> query = new TableQuery<LoopringStatsEntity>().Where(filter);
            TableContinuationToken token = null;
            var response = await _table.ExecuteQuerySegmentedAsync(query, token);

            if (response == null)
                return null;

            AllStatsDto result = new AllStatsDto();
            foreach (var item in response)
            {
                StatsDto stat = new()
                {
                    blockCount = item.blockCount,
                    nftCount = item.nftCount,
                    tradeNFTCount = item.tradeNFTCount,
                    transferCount = item.transferCount,
                    userCount = item.userCount,
                    nftMintCount = item.nftMintCount,
                    Timestamp = item.Timestamp,
                    transactionCount = item.transactionCount,
                    transferNFTCount = item.transferNFTCount
                };

                result.Stats ??= new List<StatsDto>();

                result.Stats.Add(stat);
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<AllStatsDto> GetAllDailyStatsAsync()
    {
        try
        {
            var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "LoopyStatsDaily");
            TableQuery<LoopringStatsEntity> query = new TableQuery<LoopringStatsEntity>().Where(filter);
            TableContinuationToken token = null;
            var response = await _table.ExecuteQuerySegmentedAsync(query, token);

            if (response == null)
                return null;

            AllStatsDto result = new AllStatsDto();
            foreach (var item in response)
            {
                StatsDto stat = new()
                {
                    blockCount = item.blockCount,
                    nftCount = item.nftCount,
                    tradeNFTCount = item.tradeNFTCount,
                    transferCount = item.transferCount,
                    userCount = item.userCount,
                    nftMintCount = item.nftMintCount,
                    Timestamp = item.Timestamp,
                    transactionCount = item.transactionCount,
                    transferNFTCount = item.transferNFTCount
                };

                result.Stats ??= new List<StatsDto>();

                result.Stats.Add(stat);
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
