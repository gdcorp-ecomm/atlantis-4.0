using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DataCache;
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
            GetCustomDomainSiteResponseData response = DataCache.DataCache.GetProcessRequest(request, 682) as GetCustomDomainSiteResponseData;

            Assert.AreEqual(knownPlid, response.PrivateLabelId);
        }

        [TestMethod]
        public void GetCustomDomainSiteByPLID()
        {
          string knownCustomDomainUrl = "dev.minimyn.com";
          int knownPlid = 441130;

          var request = new GetCustomDomainSitebyPLIDRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, knownPlid);
          GetCustomDomainSiteResponseData response = DataCache.DataCache.GetProcessRequest(request, 682) as GetCustomDomainSiteResponseData;

          Assert.AreEqual(knownCustomDomainUrl, response.DomainNameUrl);
        }


        [TestMethod]
        public void FailedLookupCustomDomainSite()
        {
            string bunkDomainUrl = "werewrwew.com";

            var request = new GetCustomDomainSiteByUrlRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, bunkDomainUrl);
            GetCustomDomainSiteResponseData response = DataCache.DataCache.GetProcessRequest(request, 682) as GetCustomDomainSiteResponseData;

            Assert.IsTrue(response.ResponseState == Interface.CustomDomains.CustomDomainSiteRetrieveState.FailedLookup);
        }

        [TestMethod]
        public void GetCustomDomainSiteByUrlHashTest()
        {
          string knownCustomDomainUrl = "dev.minimyn.com";
          int knownPlid = 441130;
          var request = new GetCustomDomainSiteByUrlRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, knownCustomDomainUrl);
          var hash = request.GetCacheMD5();
          Assert.AreEqual("C0-B4-98-A7-D4-38-EF-B2-52-D0-D4-24-77-89-63-21", hash);
        }

        [TestMethod]
        public void GetCustomDomainSiteByPlIDHashTest()
        {
          string knownCustomDomainUrl = "dev.minimyn.com";
          int knownPlid = 441130;
          var request = new GetCustomDomainSitebyPLIDRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, knownPlid);
          var hash = request.GetCacheMD5();
          Assert.AreEqual("2E-21-83-98-F8-50-52-D1-CD-9E-77-35-B4-D8-3A-7B", hash);
        }

    }
}
