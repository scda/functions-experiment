using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace scda.functions
{
  public static class Name
  {
    [FunctionName("Name")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest request,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      string name = request.Query["name"];

      var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
      dynamic data = JsonConvert.DeserializeObject(requestBody);
      name ??= data?.name;

      return name != null
          ? (ActionResult)new OkObjectResult($"Hello, {name}")
          : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
    }
  }
}