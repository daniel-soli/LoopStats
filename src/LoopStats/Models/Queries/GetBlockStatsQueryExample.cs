using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopStats.Models.Queries;

public class GetBlockStatsQueryExample : OpenApiExample<GetBlockStatsQuery>
{
    public override IOpenApiExample<GetBlockStatsQuery> Build(NamingStrategy namingStrategy = null)
    {
        this.Examples.Add(
            OpenApiExampleResolver.Resolve(
                "ParametersExample",
                new GetBlockStatsQuery()
                {
                    BlockId = "20123",
                    Index = 1,
                    PageSize = 25
                },
                namingStrategy
                ));
        return this;
    }
}
