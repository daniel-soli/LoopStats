using System.IO;
using System.Net;
using System.Threading.Tasks;
using LoopStats.Models.Entities;
using LoopStats.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [FunctionName("GetLatest")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Get Latest" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "response")]
        public async Task<IActionResult> GetLatest(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger GetLatest processed a request.");

            var result = await _statsRepository.GetLatestStatAsync();

            return new OkObjectResult(result);
        }

        [FunctionName("GetLastDaysCount")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Get Latest" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "response")]
        public async Task<IActionResult> GetLastDaysCount(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger GetLastDaysCount processed a request.");

            var result = await _statsRepository.GetLastDaysStatsAsync();

            return new OkObjectResult(result);
        }

        [FunctionName("GetAllQuarterlyStats")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Get Latest" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "response")]
        public async Task<IActionResult> GetAllStats(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger GetAllStats processed a request.");

            var result = await _statsRepository.GetAllAsync();

            return new OkObjectResult(result);
        }

        [FunctionName("GetAllDailyStats")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Get Daily" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "response")]
        public async Task<IActionResult> GetAllDailyStats(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger GetAllStats processed a request.");

            var result = await _statsRepository.GetAllAsync();

            return new OkObjectResult(result);
        }
    }
}

