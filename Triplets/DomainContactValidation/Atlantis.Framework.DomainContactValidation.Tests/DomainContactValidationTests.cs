using System;
using System.Collections.Generic;
using Atlantis.Framework.DomainContactValidation.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainContactValidation.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DomainContactValidation.Impl.dll")]
  public class DomainContactValidationTests
  {
    const int REQUESTID = 782;

    [TestMethod]
    public void DomainContactValidationGoodRequest1()
    {
      var dcv = new Interface.DomainContactValidation("Raj", "Vontela", string.Empty, "15500 N. Hayden Road", "Suite 100", "Scottsdale", "Ontario", "80130", "CA", "(480)-505-8800",
                                                      "(480)-505-8800", "rvontela@godaddy.com", "LGR");

      var request = new DomainContactValidationRequestData("Other", 0, dcv, new List<string> {"ca"}, 1, "es-us", new List<string>(){"test.ca"});
      var response = (DomainContactValidationResponseData)Engine.Engine.ProcessRequest(request, REQUESTID);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DomainContactValidationGoodRequestContactXml()
    {
      var dcv = new Interface.DomainContactValidation("Raj", "Vontela", string.Empty, "15500 N. Hayden Road", "Suite 100", "Scottsdale", "Arizona", "80130", "US", "(480)-505-8800",
                                                      "(480)-505-8800", "rvontela@godaddy.com", string.Empty);

      var request = new DomainContactValidationRequestData("Other", 0, dcv, new List<string>{"ca"}, 1, "en-us", new List<string>(){"test.ca"});
      var response = (DomainContactValidationResponseData)Engine.Engine.ProcessRequest(request, REQUESTID);
      var contactXml = response.ContactXml;
      Assert.AreEqual(true, !string.IsNullOrEmpty(contactXml));
    }

    [TestMethod]
    public void DomainContactValidationGoodRequestWithTrusteeIds()
    {
      var dcv = new Interface.DomainContactValidation("Raj", "Vontela", string.Empty, "15500 N. Hayden Road", "Suite 100", "Scottsdale", "Arizona", "80130", "US", "(480)-505-8800",
                                                      "(480)-505-8800", "rvontela@godaddy.com", string.Empty);

      var request = new DomainContactValidationRequestData("Other", 0, dcv, new List<string> { "fr" }, 1, "en-US", new List<string>(){"test.fr"});
      var response = (DomainContactValidationResponseData)Engine.Engine.ProcessRequest(request, REQUESTID);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DomainContactValidationGoodRequestToXml()
    {
      var dcv = new Interface.DomainContactValidation("Raj", "Vontela", string.Empty, "15500 N. Hayden Road", "Suite 100", "Scottsdale", "Arizona", "80130", "FR", "(480)-505-8800",
                                                      "(480)-505-8800", "rvontela@godaddy.com", "");

      var requestData = new DomainContactValidationRequestData("DomainTransfer", 0, dcv, new List<string> { "fr" }, 1, "fr-FR", new List<string>(){"test.fr"});
      var toXml = requestData.ToXML();
      Assert.IsTrue(!string.IsNullOrEmpty(toXml));
    }

    [TestMethod]
    public void DomainContactValidationGoodRequestGetCacheMd5()
    {
      var dcv = new Interface.DomainContactValidation("Raj", "Vontela", string.Empty, "15500 N. Hayden Road", "Suite 100", "Scottsdale", "Arizona", "80130", "CH", "(480)-505-8800",
                                                      "(480)-505-8800", "rvontela@godaddy.com", "LGR");

      var requestData = new DomainContactValidationRequestData("DomainTransfer", 0, dcv, new List<string> { "com" }, 1, "en=US", new List<string>(){"test.com"});

      try
      {
        var toXml = requestData.GetCacheMD5();
      }
      catch (Exception ex)
      {
        Assert.AreEqual(true, !string.IsNullOrEmpty(ex.Message));
      }
    }


    [TestMethod]
    public void DomainContactValidationBadRequest()
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

    [TestMethod]
    public void DomainContactValidationWithDoaminsGoodRequest()
    {
      var dcv = new Interface.DomainContactValidation("Raj", "Vontela", string.Empty, "15500 N. Hayden Road", "Suite 100", "Scottsdale", "Arizona", "80130", "FR", "(480)-505-8800",
                                                "(480)-505-8800", "rvontela@godaddy.com", "");

      var requestData = new DomainContactValidationRequestData("DomainTransfer", 0, dcv, new List<string> { "fr" }, 1, "en-US", new string[] { "oberon-1api-de.uk" });
      var toXml = requestData.ToXML();
      Assert.IsTrue(!string.IsNullOrEmpty(toXml));

      var response = (DomainContactValidationResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(!string.IsNullOrEmpty(response.ToXML()));

    }

    [TestMethod]
    public void DomainContactValidationWithMultipleDomains()
    {
      var dcv = new Interface.DomainContactValidation("John", "Doe", "Acme", "123 A St", "Ste. 1", "Toon Town", "CA",
        "12345", "US", "123-456-7890", string.Empty, "johnd@acme.com", string.Empty);

      var requestData = new DomainContactValidationRequestData("Other", 0, dcv, new string[] {"com", "net"}, 1, "en-US",
        new string[] {"test.com", "test.net"});
      var toXml = requestData.ToXML();
      Assert.IsTrue(!string.IsNullOrEmpty(toXml));

      var response = Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(!string.IsNullOrEmpty(requestData.ToXML()));
    }

    internal class XData : RequestData
    {
      internal XData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
        : base(shopperId, sourceUrl, orderId, pathway, pageCount)
      {

      }

      public override string ToXML()
      {
        return string.Empty;
      }
    }
  }
}
