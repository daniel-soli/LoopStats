
namespace LoopStats.Models.DTOs;

public class LastDayStatsDto
{
    public long blockCount { get; set; }
    public long transactionCount { get; set; }
    public long transferCount { get; set; }
    public long transferNFTCount { get; set; }
    public long tradeNFTCount { get; set; }
    public long nftMintCount { get; set; }
    public long userCount { get; set; }
    public long nftCount { get; set; }
}
