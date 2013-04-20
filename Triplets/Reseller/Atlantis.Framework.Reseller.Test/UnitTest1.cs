using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Reseller.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Reseller.Test
{
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.Reseller.Impl.dll")]
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetCustomDomainSite()
        {
            string knownCustomDomainUrl = "dev.minimyn.com";
            int knownPlid = 441130;

            GetCustomDomainSiteByUrlRequestData request = new GetCustomDomainSiteByUrlRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, knownCustomDomainUrl);
            GetCustomDomainSiteByUrlResponseData response = Engine.Engine.ProcessRequest(request, 682) as GetCustomDomainSiteByUrlResponseData;

            Assert.AreEqual(knownPlid, response.PrivateLabelId);
        }

        [TestMethod]
        public void FailedLookupCustomDomainSite()
        {
            string bunkDomainUrl = "werewrwew.com";

            GetCustomDomainSiteByUrlRequestData request = new GetCustomDomainSiteByUrlRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, bunkDomainUrl);
            GetCustomDomainSiteByUrlResponseData response = Engine.Engine.ProcessRequest(request, 682) as GetCustomDomainSiteByUrlResponseData;

            Assert.IsTrue(response.ResponseState == Interface.CustomDomains.CustomDomainSiteRetrieveState.FailedLookup);
        }
    }
}
