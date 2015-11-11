using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using TonyBaloney.St2.Client;
using TonyBaloney.St2.Client.Models;

namespace ExampleSt2Console.Controllers
{
	[RoutePrefix("api")]
    public class ActionController : ApiController
    {
	    private St2Client _st2Client;

	    public ActionController()
	    {
		    _st2Client = new St2Client(
			    "https://10.209.120.21:9100", // Auth URL 
			    "https://10.209.120.21:9101", // API URL
			    "admin",
			    "DevAdmin123",
			    true); // ignore certificate validation - if using self-signed cert
	    }

		[Route("tractor/engage")]
		[HttpPost]
	    public async Task<JsonResult<Execution>> EngageTractorBeam()
	    {
			// Get a sign-on token
		    await _st2Client.RefreshTokenAsync();

			// Any parameters needed for our action
			Dictionary<string, object> actionParameters = new Dictionary<string, object>();

			// Run our action
		    var result = await _st2Client.Executions.ExecuteActionAsync(
			    "examples.mistral-basic-two-tasks-with-notifications",
			    actionParameters);

		    return Json(result);
	    }

		[Route("doors/set")]
		[HttpPost]
		public async Task<JsonResult<Execution>> SetDoorState([FromBody]string state)
		{
			// Get a sign-on token
			await _st2Client.RefreshTokenAsync();

			// Any parameters needed for our action
			// NB: This is really really really insecure. Just an example!
			Dictionary<string, object> actionParameters = new Dictionary<string, object>
			{
				{"cmd", "echo 'Setting doors to " + state + "'"}
			};

			// Add our parameter

			// Run our action
			Execution result = await _st2Client.Executions.ExecuteActionAsync(
				"examples.mistral-basic",
				actionParameters);

			string executionId = result.id;

			// Wait to complete.
			while (result.status == "running" || result.status == "requested") 
			{
				result = await _st2Client.Executions.GetExecutionAsync(executionId);

				Thread.Sleep(20);
			}

			return Json(result);
		} 
    }
}
