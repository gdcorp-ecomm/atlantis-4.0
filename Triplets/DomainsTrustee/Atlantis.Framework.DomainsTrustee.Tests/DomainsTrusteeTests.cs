using System;
using System.Collections.Generic;
using Atlantis.Framework.DomainsTrustee.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainsTrustee.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DomainsTrustee.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainsTrustee.interface.dll")]
  public class DomainsTrusteeTests
  {
    const int REQUESTID = 781;

    [TestMethod]
    public void DomainsTrusteeRequestDataToXml()
    {
      var requestData = new DomainsTrusteeRequestData();

      var contactList = new List<DomainsTrusteeContact>
      {
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Registrant, "US"),
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Billing, "US")
      };
      var tlds = new List<string> { "info", "mobi" };
      var contactsDomains1 = new DomainsTrusteeContactsTlds(contactList, tlds);

      contactList = new List<DomainsTrusteeContact>
      {
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Registrant, "US"),
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Billing, "US"),
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Administrative, "US"),
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Technical, "US")
      };

      tlds = new List<string> { "us", "com" };
      var contactsDomains2 = new DomainsTrusteeContactsTlds(contactList, tlds);

      requestData.ContactsTldsList = new List<DomainsTrusteeContactsTlds> { contactsDomains1, contactsDomains2 };

      var toXml = requestData.ToXML();
      Assert.IsTrue(!string.IsNullOrEmpty(toXml));
      Assert.IsTrue(!string.IsNullOrEmpty(requestData.ToJson()));
    }

    [TestMethod]
    public void DomainsTrusteeRequestDataGetCacheMd5()
    {
      var requestData = new DomainsTrusteeRequestData();

      var contactList = new List<DomainsTrusteeContact>
      {
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Registrant, "US"),
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Billing, "US")
      };

      var tlds = new List<string> { "info", "mobi" };
      var contactsDomains1 = new DomainsTrusteeContactsTlds(contactList, tlds);

      requestData.ContactsTldsList = new List<DomainsTrusteeContactsTlds> { contactsDomains1 };

      var hashMd5 = requestData.GetCacheMD5();
      Assert.IsTrue(!string.IsNullOrEmpty(hashMd5));
    }

    [TestMethod]
    public void TestAvailableDomainTrustees()
    {
      var requestData = new DomainsTrusteeRequestData();

      var contactList = new List<DomainsTrusteeContact>
      {
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Registrant, "US"),
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Billing, "US"),
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Administrative, "US"),
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Technical, "US")
      };
      var tlds = new List<string> { "info", "mobi" };
      var contactsDomains1 = new DomainsTrusteeContactsTlds(contactList, tlds);

      contactList = new List<DomainsTrusteeContact>
      {
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Registrant, "US"),
        new DomainsTrusteeContact(DomainsTrusteeContactTypes.Billing, "US")
      };
      tlds = new List<string> { "us", "com" };
      var contactsDomains2 = new DomainsTrusteeContactsTlds(contactList, tlds);

      requestData.ContactsTldsList = new List<DomainsTrusteeContactsTlds> { contactsDomains1, contactsDomains2 };

      var response = (DomainsTrusteeResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(string.IsNullOrEmpty(response.ToXML()));
      Assert.IsTrue(!string.IsNullOrEmpty(response.ToJson()));

      DomainsTrusteeResponse domainTrustee;
      response.TryGetDomainTrustee("us", out domainTrustee);
      Assert.IsTrue(domainTrustee != null && domainTrustee.Tld.ToLowerInvariant() == "us" && string.IsNullOrEmpty(domainTrustee.NameWithoutExtension));
      Assert.IsTrue(!string.IsNullOrEmpty(response.ToJson()));
    }


    [TestMethod]
    public void DomainsTrusteeWithBadRequestData()
    {
      var request = new XData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      try
      {
        Engine.Engine.ProcessRequest(request, REQUESTID);
      }
      catch (Exception ex)
      {
        Assert.AreEqual(true, !string.IsNullOrEmpty(ex.Message));
      }
    }

    internal class XData : RequestData
    {
      internal XData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
        : base(shopperId, sourceURL, orderId, pathway, pageCount)
      {

      }

      public override string ToXML()
      {
        return string.Empty;
      }
    }
  }
}
