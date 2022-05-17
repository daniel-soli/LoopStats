using GraphQL;
using GraphQL.Client.Abstractions;
using LoopStats.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopStats.Services;

public class LoopringGraphConsumer : ILoopringGraphConsumer
{
    private readonly IGraphQLClient _client;

    public LoopringGraphConsumer(IGraphQLClient client)
    {
        _client = client;
    }

    public async Task<BlockStatsDto> GetBlockStats()
    {
        var query = new GraphQLRequest
        {
            Query = @"
                    query StatsQuery{
                        proxy(id: 0) {
                            blockCount
                            transactionCount
                            transferCount
                            transferNFTCount
                            tradeNFTCount
                            nftMintCount
                            userCount
                            nftCount
                        }
                    }",
            Variables = null,
            OperationName = "GetBlockStats"
        };
        var response = await _client.SendQueryAsync<Rootobject>(query);
        return response.Data.proxy;
    }

    public async Task<SingleBlockStatsDto> GetHistoricalBlockStats(int blockId)
    {
        var query = new GraphQLRequest
        {
            Query = $"query BlockQuery{{ block(id: {blockId}) {{ timestamp id transactionCount transferCount transferNFTCount tradeNFTCount nftMintCount }} }}",
            Variables = null,
            OperationName = "GetBlockStat"
        };

        var response = await _client.SendQueryAsync<SingleRootobjecct>(query);

        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(response.Data.block.timestamp));

        response.Data.block.timestamp = dateTimeOffset.ToString();

        return response.Data.block;
    }
}
