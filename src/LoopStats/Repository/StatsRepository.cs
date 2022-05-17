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

    public async Task<LoopringStatsEntity> GetLatestStatAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            TableQuery<LoopringStatsEntity> query = new TableQuery<LoopringStatsEntity>().Take(50);
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
}
