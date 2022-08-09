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
        //TODO modify struct to contain int (Counter)
        public struct Response
        {
            public string Name { get; set; }
        }

        //TODO modify reponse to read current count from request and return plus 1
        [FunctionName("IncrementCounter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var response = new Response();

            response.Name = req.Query["name"];
            if (response.Name is null) response.Name = "something";

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            response.Name = response.Name ?? data?.name;
            /*
            string responseMessage = string.IsNullOrEmpty(response.Name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";
            */
            return new OkObjectResult(response);
        }
    }
}