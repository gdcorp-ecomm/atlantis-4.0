using System.Collections.Generic;
using Atlantis.Framework.DotTypeForms.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeForms.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeForms.Impl.dll")]
  public class DotTypeFormsTests
  {
    [TestMethod]
    public void DotTypeFormsXmlGoodRequest()
    {
      var request = new DotTypeFormsXmlRequestData("dpp", 59, "MOBILE", "GA", "EN-US", 1);
      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeFormsXmlBadRequest()
    {
      var request = new DotTypeFormsXmlRequestData("dpp", -1, "name of placement", "GA", "EN-US", 1);
      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(false, response.IsSuccess);
    }

    [TestMethod]
    public void DotTypeFormsXmlPostGoodRequest1()
    {
      var request = new DotTypeFormsXmlRequestData("dpp", 71, "MOBILE", "GA", "EN-US", 1)
      {
        DotTypeFormContacts = new List<DotTypeFormContact>(4)
      };

      var contact1 = new DotTypeFormContact(DotTypeFormContactTypes.Registrant, "Raj", "Vontela", "GoDaddy",
                                            "123 Abc Rd", "Suite 45", "Scottsdale", "AZ", "85260", "US", "4805058800", "4805058800", "rvontela@gd.com");
      var contact2 = new DotTypeFormContact(DotTypeFormContactTypes.Administrative, "Rajj", "Vontelaa", "GoDaddy",
                                            "123 Abc Rdd", "Suite 455", "Scottsdalee", "AZ", "85261", "US", "4805058801", "4805058801", "rvontela@gdd.com");
      var contact3 = new DotTypeFormContact(DotTypeFormContactTypes.Billing, "Rajjj", "Vontelaaa", "GoDaddy",
                                            "123 Abc Rddd", "Suite 4555", "Scottsdaleee", "AZ", "85262", "US", "4805058802", "4805058802", "rvontela@gddd.com");
      var contact4 = new DotTypeFormContact(DotTypeFormContactTypes.Technical, "Rajjjj", "Vontelaaaa", "GoDaddy",
                                            "123 Abc Rdddd", "Suite 45555", "Scottsdaleeee", "AZ", "85263", "US", "4805058803", "4805058803", "rvontela@gdddd.com");
      request.DotTypeFormContacts.Add(contact1);
      request.DotTypeFormContacts.Add(contact2);
      request.DotTypeFormContacts.Add(contact3);
      request.DotTypeFormContacts.Add(contact4);

      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeFormsXmlPostGoodRequest2()
    {
      var request = new DotTypeFormsXmlRequestData("dpp", 59, "MOBILE", "GA", "EN-US", 1);
      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeFormsHtmlBadRequest()
    {
      var request = new DotTypeFormsHtmlRequestData("dpp", -1, "name of placement", "GA", "EN-US", 1, string.Empty);
      var response = (DotTypeFormsHtmlResponseData)Engine.Engine.ProcessRequest(request, 709);
      Assert.AreEqual(false, response.IsSuccess);
    }

    [TestMethod]
    public void DotTypeFormsHtmlGoodRequestForSmdFormType()
    {
      var request = new DotTypeFormsHtmlRequestData("trademark", 1731, "FOS", "SRA", "EN-US", 1, "fhdsjkaflhdskjaflsa.sunrise");
      var response = (DotTypeFormsHtmlResponseData)Engine.Engine.ProcessRequest(request, 709);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));

      request = new DotTypeFormsHtmlRequestData("trademark", 1731, "FOS", "SRA", "EN-US", 1, "abcd.com");
      response = (DotTypeFormsHtmlResponseData)Engine.Engine.ProcessRequest(request, 709);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }
  }
}
