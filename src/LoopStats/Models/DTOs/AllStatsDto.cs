using AutoMapper;
using LoopStats.Common.Mappings;
using LoopStats.Models.Entities;
using System;
using System.Collections.Generic;

namespace LoopStats.Models.DTOs;

public class AllStatsDto
{
    public List<StatsDto> Stats { get; set; }
}

public class StatsDto
{
    public long blockCount { get; set; }
    public long transactionCount { get; set; }
    public long transferCount { get; set; }
    public long transferNFTCount { get; set; }
    public long tradeNFTCount { get; set; }
    public long nftMintCount { get; set; }
    public long userCount { get; set; }
    public long nftCount { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
