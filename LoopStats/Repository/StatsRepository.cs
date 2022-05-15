using Microsoft.Azure.Cosmos.Table;
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

    public async Task<T> Create(T entity, CancellationToken cancellationToken = default)
    {
        var mergeOperation = TableOperation.InsertOrMerge(entity);
        var result = (await _table.ExecuteAsync(mergeOperation, cancellationToken)).Result as T;

        return result;
    }
}
