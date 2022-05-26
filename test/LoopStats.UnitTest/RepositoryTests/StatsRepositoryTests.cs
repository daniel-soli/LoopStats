using LoopStats.Models.DTOs;
using LoopStats.Models.Entities;
using LoopStats.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LoopStats.UnitTest.RepositoryTests;

public class StatsRepositoryTests
{
    [Fact]
    public async void CreateStats_WhenSendingStats_ExpectStatAdded()
    {
        // Arrange
        var mockTable = new Mock<IStatsRepository<LoopringStatsEntity>>();
        var date = DateTime.UtcNow;

        LoopringStatsEntity stats = new()
        {
            blockCount = 1,
            blockTimeStamp = date,
            nftCount = 1,
            nftMintCount = 1,
            PartitionKey = "LoopStatsQuarterly",
            RowKey = "21321654",
            tradeNFTCount = 1,
            transactionCount = 1,
            transferCount = 1,
            transferNFTCount = 1,
            userCount = 1,
            Timestamp = new DateTimeOffset(DateTime.Now),
            ETag = "W/\"datetime'2020-05-21T14%3A35%3A04.1064255Z'\""
        };


        mockTable.Setup(statsRepository => statsRepository.CreateAsync(It.IsAny<LoopringStatsEntity>(), It.IsAny<CancellationToken>()))
            .Returns(async () => await Task.FromResult(stats));

        // Act
        var result = await mockTable.Object.CreateAsync(stats);

        // Assert
        Assert.Equal(1, result.blockCount);
        Assert.Equal(1, result.nftCount);
        Assert.Equal(1, result.nftMintCount);
        Assert.Equal("LoopStatsQuarterly", result.PartitionKey);
        Assert.Equal("21321654", result.RowKey);
        Assert.Equal(1, result.tradeNFTCount);
        Assert.Equal(1, result.transactionCount);
        Assert.Equal(1, result.transferCount);
        Assert.Equal(1, result.transferNFTCount);
        Assert.Equal(1, result.userCount);
    }

    [Fact]
    public async void GetAllStatsFromQuarterly()
    {
        var mockTable = new Mock<IStatsRepository<LoopringStatsEntity>>();
        var date = DateTime.UtcNow;

        AllStatsDto stats = new()
        {
            Stats = new List<StatsDto>()
            {
                new StatsDto()
                {
                    blockCount = 21598,
                    transactionCount = 5385354,
                    transferCount = 410549,
                    transferNFTCount = 225417,
                    tradeNFTCount = 8,
                    nftMintCount = 76846,
                    userCount = 93886,
                    nftCount = 76742,
                    Timestamp = new DateTimeOffset(2022,05,26,10,00,00, new TimeSpan(1, 0, 0))
                },
                new StatsDto()
                {
                    blockCount = 21000,
                    transactionCount = 5385000,
                    transferCount = 410000,
                    transferNFTCount = 225000,
                    tradeNFTCount = 8,
                    nftMintCount = 76800,
                    userCount = 93800,
                    nftCount = 76700,
                    Timestamp = new DateTimeOffset(2022,05,26,09,00,00, new TimeSpan(1, 0, 0))
                },
                new StatsDto()
                {
                    blockCount = 20000,
                    transactionCount = 5380000,
                    transferCount = 400549,
                    transferNFTCount = 224417,
                    tradeNFTCount = 8,
                    nftMintCount = 76246,
                    userCount = 93386,
                    nftCount = 76342,
                    Timestamp = new DateTimeOffset(2022,05,26,08,00,00, new TimeSpan(1, 0, 0))
                },
            }
        };

        mockTable.Setup(statsRepository => statsRepository.GetAllAsync(It.IsAny<CancellationToken>()))
            .Returns(async () => await Task.FromResult(stats));

        // Act
        var result = await mockTable.Object.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Stats.Count);
    }
}
