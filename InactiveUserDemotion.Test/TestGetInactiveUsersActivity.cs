using InactiveUserDemotion.Activity;
using NSubstitute;
using SecurePipelineScan.VstsService;
using SecurePipelineScan.VstsService.Response;
using System;
using Xunit;

namespace InactiveUserDemotion.Test
{
    public class TestGetInactiveUsersActivity
    {
        private GetInactiveUsersActivity _activity;

        private static AccessLevel BASIC = new AccessLevel() { AccountLicenseType = "express" };
        private static AccessLevel STAKEHOLDER = new AccessLevel() { AccountLicenseType = "Stakeholder" };
        private static AccessLevel MSDN = new AccessLevel() { AccountLicenseType = "MSDN" };

        private static UserEntitlement ENTITLEMENT_FIVE_DAYS_BASIC = new UserEntitlement() { LastAccessedDate = DateTime.UtcNow.AddDays(-5), AccessLevel = BASIC };
        private static UserEntitlement ENTITLEMENT_FIVE_DAYS_STAKEHOLDER = new UserEntitlement() { LastAccessedDate = DateTime.UtcNow.AddDays(-5), AccessLevel = STAKEHOLDER };
        private static UserEntitlement ENTITLEMENT_SIXTY_DAYS_BASIC = new UserEntitlement() { LastAccessedDate = DateTime.UtcNow.AddDays(-5), AccessLevel = BASIC };
        private static UserEntitlement ENTITLEMENT_SIXTY_DAYS_STAKEHOLDER = new UserEntitlement() { LastAccessedDate = DateTime.UtcNow.AddDays(-60), AccessLevel = STAKEHOLDER };
        private static UserEntitlement ENTITLEMENT_SIXTYONE_DAYS_BASIC = new UserEntitlement() { LastAccessedDate = DateTime.UtcNow.AddDays(-61), AccessLevel = BASIC };
        private static UserEntitlement ENTITLEMENT_SIXTYONE_DAYS_STAKEHOLDER = new UserEntitlement() { LastAccessedDate = DateTime.UtcNow.AddDays(-61), AccessLevel = STAKEHOLDER };
        private static UserEntitlement ENTITLEMENT_SIXTYONE_DAYS_MSDN = new UserEntitlement() { LastAccessedDate = DateTime.UtcNow.AddDays(-61), AccessLevel = MSDN };

        private IVstsRestClient _client;

        public TestGetInactiveUsersActivity()
        {
            _client = Substitute.For<IVstsRestClient>();
            _activity = new GetInactiveUsersActivity(_client);
        }


        [Fact]
        public void TestRecentLogin()
        {
            _client.Get(Arg.Any<IEnumerableRequest<UserEntitlement>>()).Returns(
                new []
            {
                ENTITLEMENT_FIVE_DAYS_BASIC,
                ENTITLEMENT_FIVE_DAYS_STAKEHOLDER
            });

            var result = _activity.Run(null);
            Assert.Empty(result);
        }

        [Fact]
        public void TestSixtyDaysLogin()
        {
            _client.Get(Arg.Any<IEnumerableRequest<UserEntitlement>>()).Returns(
                new[]
            {
                ENTITLEMENT_SIXTY_DAYS_BASIC,
                ENTITLEMENT_SIXTY_DAYS_STAKEHOLDER
            });

            var result = _activity.Run(null);
            Assert.Empty(result);
        }

        [Fact]
        public void TestSixtyOneDaysLogin()
        {
            _client.Get(Arg.Any<IEnumerableRequest<UserEntitlement>>()).Returns(
                new[]
            {
                ENTITLEMENT_SIXTYONE_DAYS_BASIC,
                ENTITLEMENT_SIXTYONE_DAYS_STAKEHOLDER,
                ENTITLEMENT_SIXTYONE_DAYS_MSDN
            });

            var result = _activity.Run(null);
            Assert.Equal(new[] { ENTITLEMENT_SIXTYONE_DAYS_BASIC }, result);
        }
    }
}
