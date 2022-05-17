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
}
