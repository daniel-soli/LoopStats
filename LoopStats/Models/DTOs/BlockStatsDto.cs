namespace LoopStats.Models.DTOs;

public class BlockStatsDto
{
    public string blockCount { get; set; }
    public string transactionCount { get; set; }
    public string transferCount { get; set; }
    public string transferNFTCount { get; set; }
    public string tradeNFTCount { get; set; }
    public string nftMintCount { get; set; }
    public string userCount { get; set; }
    public string nftCount { get; set; }
}

public class Rootobject
{
    public BlockStatsDto proxy { get; set; }
}
