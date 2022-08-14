using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IncrementCounter
{
    public static class IncrementCounter
    {
        public struct CounterObject
        {
            public string Count { get; set; }
        }

        //TODO add a "reset counter" function
        [FunctionName("IncrementCounter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var counter = new CounterObject();

            counter.Count = req.Query["count"];
            if (counter.Count is null) return new BadRequestResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            counter.Count = counter.Count ?? data?.count;

            counter.Count = (Convert.ToInt32(counter.Count) + 1).ToString();
            /*
            string responseMessage = string.IsNullOrEmpty(response.Name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";
            */
            return new OkObjectResult(counter);
        }
    }
}
