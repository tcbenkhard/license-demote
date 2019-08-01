using InactiveUserDemotion.Activity;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SecurePipelineScan.VstsService.Response;
using System.Collections.Generic;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace InactiveUserDemotion.Orchestration
{
    public static class InactiveUserDemotionOrchestrator
    {
        [FunctionName(nameof(InactiveUserDemotionOrchestrator))]
        public static async void Run([OrchestrationTrigger] DurableOrchestrationContextBase context, ILogger logger)
        {
            var inactiveUsers = await context.CallActivityAsync<IEnumerable<UserEntitlement>>(nameof(GetInactiveUsersActivity), null);
            logger.LogInformation($"{inactiveUsers.Count()} will be demoted to a stakeholder license.");
            await Task.WhenAll(inactiveUsers.Select(i => context.CallActivityAsync(nameof(DemoteInactiveUsersActivity), i)));
        }

    }
}