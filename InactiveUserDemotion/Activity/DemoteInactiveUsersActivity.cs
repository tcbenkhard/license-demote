using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SecurePipelineScan.VstsService;
using SecurePipelineScan.VstsService.Requests;
using SecurePipelineScan.VstsService.Response;
using System;
using System.Collections.Generic;

namespace InactiveUserDemotion.Activity
{
    public class DemoteInactiveUsersActivity
    {
        private IVstsRestClient _client;
      
        public DemoteInactiveUsersActivity(IVstsRestClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        [FunctionName(nameof(DemoteInactiveUsersActivity))]
        public void Run([ActivityTrigger] DurableActivityContextBase context, UserEntitlement entitlement, ILogger logger)
        {
            logger.LogInformation($"Assigning stakeholder license to {entitlement.User.MailAddress}.");
            var stakeholderLicense = new AccessLevel() { AccountLicenseType = "stakeholder", LicensingSource = "account" };
            var patchDocument = new JsonPatchDocument().Replace("/accessLevel", stakeholderLicense);
                _client
                .PatchAsync(MemberEntitlementManagement.PatchUserEntitlements(entitlement.Id), patchDocument);
        }
    }
}
