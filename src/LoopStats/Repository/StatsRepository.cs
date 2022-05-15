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
    private readonly ILogger _log;

    public StatsRepository(CloudTable table, ILogger log)
    {
        _table = table;
        _log = log;
    }

    public async Task<T> Create(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            var mergeOperation = TableOperation.InsertOrMerge(entity);
            var result = (await _table.ExecuteAsync(mergeOperation, cancellationToken)).Result as T;

            _log.LogInformation("Saved to storage");

            return result;
        }
        catch (Exception ex)
        {
            _log.LogError($"Error when saving to storage. {ex.Message}");
            return null;
        }
    }
}
