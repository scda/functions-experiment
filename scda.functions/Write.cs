using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace scda.functions
{
  public static class Write
  {
    [FunctionName("ToDo")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request,
        [CosmosDB(
                "ToDoList",
                "Items",
                ConnectionStringSetting = "TODO_COSMOSDB_CONNECTIONSTRING")] // read from settings.json or application settings
            out dynamic document,
        ILogger log)
    {
      log.LogInformation("C# HTTP Trigger to CosmosDB");

      var requestBody = new StreamReader(request.Body).ReadToEnd();
      dynamic data = JsonConvert.DeserializeObject(requestBody);
      var item = data?.item;

      var documentId = Guid.NewGuid();
      document = new { Description = item, id = documentId };

      return item != null
          ? (ActionResult)new OkObjectResult($"saved item : [{documentId}] {item}")
          : new BadRequestObjectResult("Please pass in a body with an `item`.");
    }
  }
}