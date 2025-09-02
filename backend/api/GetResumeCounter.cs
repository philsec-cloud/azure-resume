using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;

namespace Company.Function
{
    public class GetResumeCounter
    {
        private readonly ILogger _logger;
        private readonly CosmosClient _cosmosClient;

        public GetResumeCounter(ILoggerFactory loggerFactory, CosmosClient cosmosClient)
        {
            _logger = loggerFactory.CreateLogger<GetResumeCounter>();
            _cosmosClient = cosmosClient;
        }

        [Function("GetResumeCounter")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("Processing GetResumeCounter Function...");

            try
            {
                // Get the container
                var container = _cosmosClient.GetContainer("AzureResume", "Counter");

                // Read the counter document
                var counterResponse = await container.ReadItemAsync<Counter>("1", new PartitionKey("1"));
                var counter = counterResponse.Resource;

                // Increments counter
                counter.Count += 1;

                // Update the counter in CosmosDB
                await container.UpsertItemAsync(counter, new PartitionKey("1"));

                // Prepare HTTP response
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync(JsonConvert.SerializeObject(counter));

                return response;
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"CosmosDB error: {ex.Message}");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Error accessing database");
                return errorResponse;
            }
        }
    }
}