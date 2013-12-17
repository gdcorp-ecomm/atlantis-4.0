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

      var request = new DomainContactValidationRequestData(DomainCheckType.Other, DomainContactType.Registrant, dcv, "ca", 1);
      var response = (DomainContactValidationResponseData)Engine.Engine.ProcessRequest(request, REQUESTID);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DomainContactValidationGoodRequestContactXml()
    {
      var dcv = new Interface.DomainContactValidation("Raj", "Vontela", string.Empty, "15500 N. Hayden Road", "Suite 100", "Scottsdale", "Ontario", "80130", "CA", "(480)-505-8800",
                                                      "(480)-505-8800", "rvontela@godaddy.com", "LGR");

      var request = new DomainContactValidationRequestData(DomainCheckType.Other, DomainContactType.Registrant, dcv, "ca", 1);
      var response = (DomainContactValidationResponseData)Engine.Engine.ProcessRequest(request, REQUESTID);
      var contactXml = response.ContactXml;
      Assert.AreEqual(true, !string.IsNullOrEmpty(contactXml));
    }

    [TestMethod]
    public void DomainContactValidationGoodRequestWithTrusteeIds()
    {
      var dcv = new Interface.DomainContactValidation("Raj", "Vontela", string.Empty, "15500 N. Hayden Road", "Suite 100", "Scottsdale", "Arizona", "80130", "US", "(480)-505-8800",
                                                      "(480)-505-8800", "rvontela@godaddy.com", string.Empty);

      var request = new DomainContactValidationRequestData(DomainCheckType.Other, DomainContactType.Registrant, dcv, "fr", 1);
      var response = (DomainContactValidationResponseData)Engine.Engine.ProcessRequest(request, REQUESTID);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DomainContactValidationGoodRequestToXml()
    {
      var dcv = new Interface.DomainContactValidation("Raj", "Vontela", string.Empty, "15500 N. Hayden Road", "Suite 100", "Scottsdale", "Arizona", "80130", "FR", "(480)-505-8800",
                                                      "(480)-505-8800", "rvontela@godaddy.com", "");

      var requestData = new DomainContactValidationRequestData(DomainCheckType.DomainTransfer, DomainContactType.Registrant, dcv, "fr", 1);
      var toXml = requestData.ToXML();
      Assert.IsTrue(!string.IsNullOrEmpty(toXml));
    }

    [TestMethod]
    public void DomainContactValidationGoodRequestGetCacheMd5()
    {
      var dcv = new Interface.DomainContactValidation("Raj", "Vontela", string.Empty, "15500 N. Hayden Road", "Suite 100", "Scottsdale", "Arizona", "80130", "CH", "(480)-505-8800",
                                                      "(480)-505-8800", "rvontela@godaddy.com", "LGR");

      var requestData = new DomainContactValidationRequestData(DomainCheckType.DomainTransfer, DomainContactType.Registrant, dcv, "com", 1);

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
