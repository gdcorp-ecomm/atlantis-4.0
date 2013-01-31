
using Atlantis.Framework.MobilePushEmailGetSub.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Engine;
using System;

namespace Atlantis.Framework.MobilePushEmailGetSub.Tests
{
    [TestClass]
    public class MobilePushEmailGetSubscriptionsTests
    {
        const int _REQUESTID = 645;
        private const string VALID_EMAIL = "andy2@123-weight.com";
        private const string VALID_EMAIL_NO_SUBS = "smash@dang-a-lang.com";
        private const string INVALID_EMAIL = "heyayayay@huyaahayayaha.hoya";

        [TestMethod]
        [DeploymentItem("atlantis.config")]
        [DeploymentItem("Atlantis.Framework.MobilePushEmailGetSubscriptions.Impl.dll")]
        public void MobilePushEmailGetSubscriptionsValidEmail()
        {
            MobilePushEmailGetSubscriptionsRequestData request = new MobilePushEmailGetSubscriptionsRequestData(VALID_EMAIL, "", "", string.Empty, string.Empty, string.Empty, 0);
            MobilePushEmailGetSubscriptionsResponseData response = (MobilePushEmailGetSubscriptionsResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Subscriptions);
            Assert.IsTrue(response.Subscriptions.Length>0);
            Assert.IsTrue(response.LoginExists);
        }

        [TestMethod]
        [DeploymentItem("atlantis.config")]
        [DeploymentItem("Atlantis.Framework.MobilePushEmailGetSubscriptions.Impl.dll")]
        public void MobilePushEmailGetSubscriptionsInvalidEmail()
        {
            MobilePushEmailGetSubscriptionsRequestData request = new MobilePushEmailGetSubscriptionsRequestData(INVALID_EMAIL, "", "", string.Empty, string.Empty, string.Empty, 0);
            MobilePushEmailGetSubscriptionsResponseData response = (MobilePushEmailGetSubscriptionsResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
            Assert.IsNotNull(response);
            Assert.IsNull(response.Subscriptions);
            Assert.IsFalse(response.LoginExists);
        }

        [TestMethod]
        [DeploymentItem("atlantis.config")]
        [DeploymentItem("Atlantis.Framework.MobilePushEmailGetSubscriptions.Impl.dll")]
        public void MobilePushEmailGetSubscriptionsValidEmailNoSubscriptions()
        {
            MobilePushEmailGetSubscriptionsRequestData request = new MobilePushEmailGetSubscriptionsRequestData(VALID_EMAIL_NO_SUBS, "", "", string.Empty, string.Empty, string.Empty, 0);
            MobilePushEmailGetSubscriptionsResponseData response = (MobilePushEmailGetSubscriptionsResponseData)Engine.Engine.ProcessRequest(request, _REQUESTID);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Subscriptions);
            Assert.IsTrue(response.Subscriptions.Length == 0);
            Assert.IsTrue(response.LoginExists);
        }

    }
}