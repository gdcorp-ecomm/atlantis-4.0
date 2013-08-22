using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainLookup;
using Atlantis.Framework.Providers.DomainLookup.Interface;
using Atlantis.Framework.Testing.MockProviders;

namespace Atlantis.Framework.Providers.DomainLookup.Tests
{
    [TestClass]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainLookup.Impl.dll")]
    public class DomainLookupProviderTests
    {
        MockProviderContainer _container = new MockProviderContainer();

        private IDomainLookupProvider NewDomainLookupProvider()
        {
            _container.RegisterProvider<IDomainLookupProvider, DomainLookupProvider>();
            return _container.Resolve<IDomainLookupProvider>();
        }

        [TestMethod]
        public void CheckActiveDomain()
        {
            IDomainLookupProvider provider = NewDomainLookupProvider();

            IDomainLookupResponse response = provider.GetDomainInformation("jeffmcookietest1.info");

            DateTime xferAwayDate = DateTime.MinValue;
            DateTime.TryParse("2013-03-18T07:58:23-07:00", out xferAwayDate);

            DateTime createDate = DateTime.MinValue;
            DateTime.TryParse("2013-01-17T14:59:06-07:00", out createDate);

            Assert.AreEqual(response.XfrAwayDateUpdateReason, 1);
            Assert.AreEqual(response.XfrAwayDate, xferAwayDate);
            Assert.AreEqual(response.CreateDate, createDate);
            Assert.AreEqual(response.IsActive, true);
            bool privateLabelCheck = false;

            if (response.PrivateLabelID == 1)
                privateLabelCheck = true;

            Assert.IsTrue(privateLabelCheck);
        }

        [TestMethod]
        public void CheckActiveResellerDomain()
        {
            IDomainLookupProvider provider = NewDomainLookupProvider();
            IDomainLookupResponse response = provider.GetDomainInformation("ELEVENCATS.INFO");

            Assert.AreEqual(response.DomainID, 2146871);
            Assert.AreEqual(response.HasSuspectTerms, false);
            Assert.AreEqual(response.IsActive, true);
            bool privateLabelCheck = false;

            if (response.PrivateLabelID > 3)
                privateLabelCheck = true;

            Assert.IsTrue(privateLabelCheck);
        }

        [TestMethod]
        public void CheckForEmptyResponse()
        {
            IDomainLookupProvider provider = NewDomainLookupProvider();
            IDomainLookupResponse response = provider.GetDomainInformation("gghhasdd");

            Assert.AreEqual(response.Shopperid, "");
            Assert.AreEqual(response.IsActive, false);
            Assert.AreEqual(response.IsSmartDomain, false);
        }
    }
}
