﻿using AutoMapper;
using LoopStats.Common.Mappings;
using LoopStats.Models.DTOs;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace LoopStats.Models.Entities;

public class LoopringStatsEntity : TableEntity, IMapFrom<BlockStatsDto>, IMapFrom<SingleBlockStatsDto>
{
    public long blockCount { get; set; }
    public long transactionCount { get; set; }
    public long transferCount { get; set; }
    public long transferNFTCount { get; set; }
    public long tradeNFTCount { get; set; }
    public long nftMintCount { get; set; }
    public long userCount { get; set; }
    public long nftCount { get; set; }
    public DateTime blockTimeStamp { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<BlockStatsDto, LoopringStatsEntity>();
        profile.CreateMap<SingleBlockStatsDto, LoopringStatsEntity>()
            .ForMember(d => d.blockCount, opt => opt.MapFrom(x => x.id))
            .ForMember(d => d.Timestamp, opt => opt.MapFrom(x => x.timestamp));
    }
}
