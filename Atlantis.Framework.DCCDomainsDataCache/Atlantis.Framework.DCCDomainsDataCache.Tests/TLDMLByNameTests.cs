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
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "COM.AU");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    public void TLDMLFoundLowerCase()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com.au");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void TLDMLNotFound()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "INFO");
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
    public void MinRegistrationOrg()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "org");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Assert.AreNotEqual(0, response.Product.RegistrationYears.Min);
    }

    [TestMethod]
    public void MaxRegistrationOrg()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Org");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Assert.AreNotEqual(0, response.Product.RegistrationYears.Max);
    }

    [TestMethod]
    public void TLDMLFoundOrg()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "ORG");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    public void MinRegistrationComAu()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com.au");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Assert.AreEqual(2, response.Product.RegistrationYears.Min);
      Assert.AreEqual(2, response.Product.RegistrationYears.Max);
    }

    [TestMethod]
    public void NoPreregLengthsOrg()
    {
      var request = new TLDMLByNameRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "ORG");
      var response = (TLDMLByNameResponseData)DataCache.DataCache.GetProcessRequest(request, _GETBYNAMEREQUEST);
      Assert.AreEqual(response.Product.PreregistrationYears("SRA"), TldValidYearsSet.INVALIDSET);
    }


  }
}
