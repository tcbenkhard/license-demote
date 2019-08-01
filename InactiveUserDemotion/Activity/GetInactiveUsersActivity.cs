using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SecurePipelineScan.VstsService;
using SecurePipelineScan.VstsService.Requests;
using SecurePipelineScan.VstsService.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InactiveUserDemotion.Activity
{
    public class GetInactiveUsersActivity
    {
        private IVstsRestClient _client;
        private const int DAYS_OF_INACTIVITY_TRESHOLD = 60;

        public GetInactiveUsersActivity(IVstsRestClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        [FunctionName(nameof(GetInactiveUsersActivity))]
        public IEnumerable<UserEntitlement> Run([ActivityTrigger] DurableActivityContextBase context) =>
            _client.Get(MemberEntitlementManagement.UserEntitlements())
                .Where(e => e.LastAccessedDate < DateTime.UtcNow.AddDays(-DAYS_OF_INACTIVITY_TRESHOLD))
                .Where(e => e.AccessLevel.AccountLicenseType.Equals("express"));

    }
}
