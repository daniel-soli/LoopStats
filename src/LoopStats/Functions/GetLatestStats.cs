using System.IO;
using System.Net;
using System.Threading.Tasks;
using LoopStats.Models.DTOs;
using LoopStats.Models.Entities;
using LoopStats.Models.Queries;
using LoopStats.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace LoopStats.Functions
{
    public class GetLatestStats
    {
        private readonly ILogger<GetLatestStats> _logger;
        private readonly IStatsRepository<LoopringStatsEntity> _statsRepository;

        public GetLatestStats(ILogger<GetLatestStats> log, IStatsRepository<LoopringStatsEntity> statsRepository)
        {
            _logger = log;
            _statsRepository = statsRepository;
        }

        [FunctionName(nameof(GetLatest))]
        [OpenApiOperation(operationId: "Latest", tags: new[] { "Get Latest" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "response")]
        public async Task<IActionResult> GetLatest(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger GetLatest processed a request.");

            var result = await _statsRepository.GetLatestStatAsync();

            return new OkObjectResult(result);
        }

        [FunctionName(nameof(GetLastDaysCount))]
        [OpenApiOperation(operationId: "Count", tags: new[] { "Get Last Day Count" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "response")]
        public async Task<IActionResult> GetLastDaysCount(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger GetLastDaysCount processed a request.");

            var result = await _statsRepository.GetLastDaysStatsAsync();

            return new OkObjectResult(result);
        }

        [FunctionName(nameof(GetBlocksQuery))]
        [OpenApiOperation(operationId: "All", tags: new[] { "Get All Stats" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "response")]
        [OpenApiParameter(name: "BlockId", In = ParameterLocation.Query, Type = typeof(string), Description = "Block Id")]
        [OpenApiParameter(name: "PageIndex", In = ParameterLocation.Query, Type = typeof(int), Description = "Page Index")]
        [OpenApiParameter(name: "PageSize", In = ParameterLocation.Query, Type = typeof(int), Description = "Page Size")]
        public async Task<ActionResult<PaginatedList<StatsDto>>> GetBlocksQuery(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger GetAllStats processed a request.");

            GetBlockStatsQuery query = new()
            {
                BlockId = string.IsNullOrWhiteSpace(req.Query["BlockId"]) ? "" : req.Query["BlockId"],
                Index = int.TryParse(req.Query["PageIndex"], out int index) ? index : 1,
                PageSize = int.TryParse(req.Query["PageSize"], out int size) ? size : 25
            };

            var result = await _statsRepository.GetBlocksQuery(query);

            return new OkObjectResult(result);
        }

        [FunctionName(nameof(GetAllDailyStats))]
        [OpenApiOperation(operationId: "Daily", tags: new[] { "Get Daily" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "response")]
        public async Task<IActionResult> GetAllDailyStats(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger GetAllDailyStats processed a request.");

            var result = await _statsRepository.GetAllDailyStatsAsync();

            return new OkObjectResult(result);
        }

        [FunctionName(nameof(GetDailyCountFromMidnightUTC))]
        [OpenApiOperation(operationId: "DailyCount", tags: new[] { "Get daily count from 00:30" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "response")]
        public async Task<IActionResult> GetDailyCountFromMidnightUTC(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger GetDailyCountFromMidnightUTC processed a request.");

            var result = await _statsRepository.GetCountFromToday();

            return new OkObjectResult(result);
        }
    }
}

