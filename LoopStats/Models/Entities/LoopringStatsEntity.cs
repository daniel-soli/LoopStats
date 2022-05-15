using AutoMapper;
using LoopStats.Common.Mappings;
using LoopStats.Models.DTOs;
using Microsoft.Azure.Cosmos.Table;

namespace LoopStats.Models.Entities;

public class LoopringStatsEntity : TableEntity, IMapFrom<BlockStatsDto>
{
    public string blockCount { get; set; }
    public string transactionCount { get; set; }
    public string transferCount { get; set; }
    public string transferNFTCount { get; set; }
    public string tradeNFTCount { get; set; }
    public string nftMintCount { get; set; }
    public string userCount { get; set; }
    public string nftCount { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<BlockStatsDto, LoopringStatsEntity>();
    }
}
