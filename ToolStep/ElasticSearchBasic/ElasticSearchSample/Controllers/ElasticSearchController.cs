using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchSample.Controllers
{
    /// <summary>
    /// https://www.c-sharpcorner.com/article/logging-with-elasticsearch-kibana-serilog-using-asp-net-core-docker/
    /// </summary>
    [Route("api/[controller]")]  
    public class ElasticSearchController : ControllerBase
    {

        private readonly ILogger<ElasticSearchController> _logger;

        public ElasticSearchController(ILogger<ElasticSearchController> logger)
        {
            _logger = logger;
        }

        // GET: api/values  
        [HttpGet]
        public int GetRandomvalue()
        {
            var random = new Random();
            var randomValue = random.Next(0, 100);
            _logger.LogInformation($"Random Value is {randomValue}");
            return randomValue;
        }
    }
}
