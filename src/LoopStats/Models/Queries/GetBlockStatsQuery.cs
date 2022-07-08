using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace LoopStats.Models.Queries;

public class GetBlockStatsQuery
{
    public string BlockId { get; set; }
    public int Index { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
