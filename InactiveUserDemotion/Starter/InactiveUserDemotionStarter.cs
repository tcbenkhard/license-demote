using System;
using System.Threading.Tasks;
using InactiveUserDemotion.Orchestration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace InactiveUserDemotion
{
    public class InactiveUserDemotionStarter
    {
        [FunctionName("InactiveUserDemotionStarter")]
        public async Task Run([TimerTrigger("0 59 23 * * *")]TimerInfo myTimer,
            ILogger log, 
            [OrchestrationClient] DurableOrchestrationClientBase orchestrationClient)
        {
            await orchestrationClient.StartNewAsync(nameof(InactiveUserDemotionOrchestrator), null);
        }
    }
}
