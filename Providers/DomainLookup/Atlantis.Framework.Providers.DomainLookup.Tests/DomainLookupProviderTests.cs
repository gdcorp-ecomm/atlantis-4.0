using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Providers.DomainLookup.Interface;
using Atlantis.Framework.Testing.MockProviders;

namespace Atlantis.Framework.Providers.DomainLookup.Tests
{
    [TestClass]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainLookup.Impl.dll")]
    public class DomainLookupProviderTests
    {
      private readonly MockProviderContainer _container = new MockProviderContainer();

      private IDomainLookupProvider NewDomainLookupProvider()
      {
        _container.RegisterProvider<IDomainLookupProvider, DomainLookupProvider>();
        return _container.Resolve<IDomainLookupProvider>();
      }

      [TestMethod]
      public void CheckActiveDomain()
      {
        IDomainLookupProvider provider = NewDomainLookupProvider();

        IDomainLookupData data = provider.GetDomainInformation("jeffmcookietest1.info");

        DateTime xferAwayDate = DateTime.MinValue;
        DateTime.TryParse("2013-03-18T07:58:23-07:00", out xferAwayDate);

        DateTime createDate = DateTime.MinValue;
        DateTime.TryParse("2013-01-17T14:59:06-07:00", out createDate);

        Assert.AreEqual(data.XfrAwayDateUpdateReason, 1);
        Assert.AreEqual(data.XfrAwayDate, xferAwayDate);
        Assert.AreEqual(data.CreateDate, createDate);
        Assert.AreEqual(data.IsActive, true);
        bool privateLabelCheck = false;

        if (data.PrivateLabelId == 1)
          privateLabelCheck = true;

        Assert.IsFalse(provider.IsDomainExpired());
        Assert.IsFalse(provider.IsDomainWithin90DaysOfExpiration());
        Assert.IsTrue(provider.ParkedDomainInfo.IsActive);
        Assert.IsTrue(privateLabelCheck);
        Assert.AreEqual("jeffmcookietest1.info", provider.DomainName);
        Assert.AreEqual(data.PdDomainId, 467553);
      }

      [TestMethod]
      public void CheckActiveResellerDomain()
      {
        IDomainLookupProvider provider = NewDomainLookupProvider();
        IDomainLookupData data = provider.GetDomainInformation("ELEVENCATS.INFO");

        Assert.AreEqual(data.DomainId, 2146871);
        Assert.AreEqual(data.HasSuspectTerms, false);
        Assert.AreEqual(data.IsActive, true);
        bool privateLabelCheck = false;

        if (data.PrivateLabelId > 3)
          privateLabelCheck = true;

        Assert.IsTrue(privateLabelCheck);

        Assert.AreEqual(data.PdDomainId, 477322);
      }

      [TestMethod]
      public void CheckForEmptyResponse()
      {
        IDomainLookupProvider provider = NewDomainLookupProvider();
        IDomainLookupData data = provider.GetDomainInformation("gghhasdd");

        Assert.AreEqual(data.Shopperid, string.Empty);
        Assert.AreEqual(data.IsActive, false);
        Assert.AreEqual(data.IsSmartDomain, false);
      }

      [TestMethod]
      public void NullDomain()
      {
        IDomainLookupProvider provider = NewDomainLookupProvider();
        IDomainLookupData data = provider.GetDomainInformation(null);

        Assert.AreEqual(data.Shopperid, string.Empty);
        Assert.AreEqual(data.IsActive, false);
        Assert.AreEqual(data.IsSmartDomain, false);
      }

      [TestMethod]
      public void EmptyDomain()
      {
        IDomainLookupProvider provider = NewDomainLookupProvider();
        IDomainLookupData data = provider.GetDomainInformation(string.Empty);

        Assert.AreEqual(data.Shopperid, string.Empty);
        Assert.AreEqual(data.IsActive, false);
        Assert.AreEqual(data.IsSmartDomain, false);
      }
    }
}
