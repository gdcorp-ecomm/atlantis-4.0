using Atlantis.Framework.DCCDomainsDataCache.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.DCCDomainsDataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  public class TLDMLByNameTests
  {
    const int _GETBYNAMEREQUEST = 634;

    [TestMethod]
    public void TLDMLFoundUpperCase()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "BUILD");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    public void TLDMLFoundLowerCase()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "build");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void TLDMLNotFound()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "ORG");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void TLDMLNull()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null);
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void TLDMLEmptyString()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty);
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void MinRegistrationOrg()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "org");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Assert.AreNotEqual(0, response.Product.RegistrationYears.Min);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void MaxRegistrationOrg()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Org");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Assert.AreNotEqual(0, response.Product.RegistrationYears.Max);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void TLDMLFoundOrg()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "ORG");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    public void MinRegistrationComAu()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "build");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Assert.AreEqual(1, response.Product.RegistrationYears.Min);
      Assert.AreEqual(10, response.Product.RegistrationYears.Max);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void NoPreregLengthsOrg()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "ORG");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Assert.AreEqual(response.Product.PreregistrationYears("SRA"), TldValidYearsSet.INVALIDSET);
    }

    [TestMethod]
    public void GetAllClientRequestPhases()
    {
      var result = false;
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "BUILD");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);

      var allPhases = response.Phase.GetAllLaunchPhases();

      if (allPhases.Count > 0)
      {
        result = true;
      }

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetActiveClientRequestPhases()
    {
      var result = false;
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "BUILD");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);

      var allPhases = response.Phase.GetAllLaunchPhases(true);

      if (allPhases.Count > 0)
      {
        result = true;
      }

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetNoActiveClientRequestPhases()
    {
      var result = false;
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "SUNRISE-0911");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);

      var allPhases = response.Phase.GetAllLaunchPhases(true);

      if (allPhases.Count == 0)
      {
        result = true;
      }

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TrusteeRequiredTestSG()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "SG");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Assert.IsTrue(response.Product.Trustee.IsRequired);
    }

    [TestMethod]
    public void RegistryPremiumDomainsTestReviews()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "REVIEWS");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Assert.IsTrue(response.Product.RegistryPremiumDomains.DefaultPremiumTier > 0);
    }

    [TestMethod]
    public void TrusteeVendorIdsTestSG()
    {
        var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "SG");
        var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
        Assert.IsTrue(response.Product.Trustee.TrusteeVendorId > 0);
    }
  }
}