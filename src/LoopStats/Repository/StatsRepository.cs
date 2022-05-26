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

    public StatsRepository(CloudTable table)
    {
        _table = table;
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

    public async Task<LoopringStatsEntity> GetByBlockId(long blockId, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "LoopyStatsQuarterly");
            var filter2 = TableQuery.GenerateFilterConditionForLong("blockCount", QueryComparisons.Equal, blockId);

            var combined = TableQuery.CombineFilters(filter, TableOperators.And, filter2);
            TableQuery<LoopringStatsEntity> query = new TableQuery<LoopringStatsEntity>().Where(combined);
            TableContinuationToken token = null;
            var response = await _table.ExecuteQuerySegmentedAsync(query, token);

            var result = response.LastOrDefault();

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
            DateTime lastDay = DateTime.UtcNow.AddDays(-1);
            var filter2 = TableQuery.GenerateFilterConditionForDate("blockTimeStamp", QueryComparisons.GreaterThan, lastDay);
            var combined = TableQuery.CombineFilters(filter, TableOperators.And, filter2);
            TableQuery<LoopringStatsEntity> query = new TableQuery<LoopringStatsEntity>().Where(combined);
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

    public async Task<AllStatsDto> GetAllDailyStatsAsync(CancellationToken cancellationToken = default)
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


    /// <summary>
    /// Gets the latest block counts and the daily (at 00:30) and count the difference. 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// Returns the count from difference between latest and latest daily.
    /// </returns>
    /// <exception cref="Exception"></exception>
    public async Task<LastDayStatsDto> GetCountFromToday(CancellationToken cancellationToken = default)
    {
        try
        {
            // Get the latest block
            var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "LoopyStatsQuarterly");
            TableQuery<LoopringStatsEntity> query = new TableQuery<LoopringStatsEntity>().Where(filter);
            TableContinuationToken token = null;
            var response = await _table.ExecuteQuerySegmentedAsync(query, token);

            var latestBlock = response.FirstOrDefault();

            // Get the latest daily
            var filter2 = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "LoopyStatsDaily");
            TableQuery<LoopringStatsEntity> query2 = new TableQuery<LoopringStatsEntity>().Where(filter2);
            TableContinuationToken token2 = null;
            var response2 = await _table.ExecuteQuerySegmentedAsync(query2, token2);

            var latestDaily = response2.FirstOrDefault();

            LastDayStatsDto result = new LastDayStatsDto()
            {
                blockCount = latestBlock.blockCount - latestDaily.blockCount,
                nftCount = latestBlock.nftCount - latestDaily.nftCount,
                nftMintCount = latestBlock.nftMintCount - latestDaily.nftMintCount,
                tradeNFTCount = latestBlock.tradeNFTCount - latestDaily.tradeNFTCount,
                transactionCount = latestBlock.transactionCount - latestDaily.transactionCount,
                transferCount = latestBlock.transferCount - latestDaily.transferCount,
                transferNFTCount = latestBlock.transferNFTCount - latestDaily.transferNFTCount,
                userCount = latestBlock.userCount - latestDaily.userCount
            };

            return result;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
