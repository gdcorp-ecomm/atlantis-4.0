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

            var request = new GetCustomDomainSiteByUrlRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, knownCustomDomainUrl);
            GetCustomDomainSiteResponseData response = Engine.Engine.ProcessRequest(request, 682) as GetCustomDomainSiteResponseData;

            Assert.AreEqual(knownPlid, response.PrivateLabelId);
        }

        [TestMethod]
        public void GetCustomDomainSiteByPLID()
        {
          string knownCustomDomainUrl = "dev.minimyn.com";
          int knownPlid = 441130;

          var request = new GetCustomDomainSitebyPLIDRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, knownPlid);
          GetCustomDomainSiteResponseData response = Engine.Engine.ProcessRequest(request, 682) as GetCustomDomainSiteResponseData;

          Assert.AreEqual(knownCustomDomainUrl, response.DomainNameUrl);
        }


        [TestMethod]
        public void FailedLookupCustomDomainSite()
        {
            string bunkDomainUrl = "werewrwew.com";

            var request = new GetCustomDomainSiteByUrlRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, bunkDomainUrl);
            GetCustomDomainSiteResponseData response = Engine.Engine.ProcessRequest(request, 682) as GetCustomDomainSiteResponseData;

            Assert.IsTrue(response.ResponseState == Interface.CustomDomains.CustomDomainSiteRetrieveState.FailedLookup);
        }
    }
}
