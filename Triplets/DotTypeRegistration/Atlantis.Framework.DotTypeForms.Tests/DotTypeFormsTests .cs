using System;
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
    // https://tldml.dev.int.godaddy.com/groupsmanager
    // https://tldml.test.int.godaddy.com/groupsmanager

    [TestMethod]
    public void DotTypeFormsXmlRequestForComDotPtEligibility()
    {
      // TldId = 356 is .COM.PT
      var request = new DotTypeFormsXmlRequestData("dpp", 356, "MOBILE", "GA", "EN-US", 1)
      {
        DotTypeFormContacts = new List<DotTypeFormContact>(4),
        RequestTimeout = TimeSpan.FromSeconds(15)
      };

      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Registrant));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Administrative));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Billing));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Technical));

      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(response.ToXML().Contains("validationrule name=\"RPTAdminVatIdVAT\""));
    }

    [TestMethod]
    public void DotTypeFormsXmlRequestForPtEligibility()
    {
      // TldId = 193 is .PT
      var request = new DotTypeFormsXmlRequestData("dpp", 193, "MOBILE", "GA", "EN-US", 1)
      {
        DotTypeFormContacts = new List<DotTypeFormContact>(4),
        RequestTimeout = TimeSpan.FromSeconds(15)
      };

      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Registrant));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Administrative));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Billing));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Technical));

      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(response.ToXML().Contains("validationrule name=\"RPTAdminVatIdVAT\""));
    }

    [TestMethod]
    public void DotTypeFormsXmlRequestForClEligibility()
    {
      // TldId = 59 is .CL
      var request = new DotTypeFormsXmlRequestData("dpp", 59, "MOBILE", "GA", "EN-US", 1)
      {
        DotTypeFormContacts = new List<DotTypeFormContact>(4),
        RequestTimeout = TimeSpan.FromSeconds(15)
      };

      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Registrant));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Administrative));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Billing));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Technical));

      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(response.ToXML().Contains("validationrule name=\"RClRutNumber\""));
    }

    [TestMethod]
    public void DotTypeFormsXmlRequestForClTrustee()
    {
      // TldId = 59 is .CL
      var request = new DotTypeFormsXmlRequestData("trustee", 59, "MOBILE", "GA", "EN-US", 1)
      {
        DotTypeFormContacts = new List<DotTypeFormContact>(4),
        RequestTimeout = TimeSpan.FromSeconds(15)
      };

      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Registrant));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Administrative));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Billing));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Technical));

      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(response.ToXML().Contains("validationrule name=\"RClAcceptTrustee\""));
    }

    [TestMethod]
    public void DotTypeFormsXmlRequestForDkEligibility()
    {
      // TldId = 71 is .DK
      // dpp = eligibility
      var request = new DotTypeFormsXmlRequestData("dpp", 71, "MOBILE", "GA", "EN-US", 1)
      {
        DotTypeFormContacts = new List<DotTypeFormContact>(4),
        RequestTimeout = TimeSpan.FromSeconds(15)
      };

      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Registrant));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Administrative));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Billing));
      request.DotTypeFormContacts.Add(GetGeneralContact(DotTypeFormContactTypes.Technical));

      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(!string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeFormsXmlPostGoodRequestForNycEligibility()
    {
      // TldId = 2021 is .NYC
      var request = new DotTypeFormsXmlRequestData("dpp", 2021, "fos", "GA", "EN-US", 1)
      {
        DotTypeFormContacts = new List<DotTypeFormContact>(4),
        RequestTimeout = TimeSpan.FromSeconds(15)
      };

      request.DotTypeFormContacts.Add(GetNYCContact(DotTypeFormContactTypes.Registrant));
      request.DotTypeFormContacts.Add(GetNYCContact(DotTypeFormContactTypes.Administrative));
      request.DotTypeFormContacts.Add(GetNYCContact(DotTypeFormContactTypes.Billing));
      request.DotTypeFormContacts.Add(GetNYCContact(DotTypeFormContactTypes.Technical));

      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(response.ToXML().Contains("validationrule name=\"RNycExtContactType\""));
    }

    [TestMethod]
    public void DotTypeFormsXmlBadRequest()
    {
      var request = new DotTypeFormsXmlRequestData("dpp", -1, "name of placement", "GA", "EN-US", 1);
      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.IsTrue(string.IsNullOrEmpty(response.ToXML()));
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

    [TestMethod]
    public void DotTypeFormsToJsonContainsRequiredFlag()
    {
      // TldId = 2021 is .NYC
      var request = new DotTypeFormsXmlRequestData("dpp", 2021, "fos", "GA", "EN-US", 1)
        {
          DotTypeFormContacts = new List<DotTypeFormContact>(4),
          RequestTimeout = TimeSpan.FromSeconds(15)
        };

      request.DotTypeFormContacts.Add(GetNYCContact(DotTypeFormContactTypes.Registrant));
      request.DotTypeFormContacts.Add(GetNYCContact(DotTypeFormContactTypes.Administrative));
      request.DotTypeFormContacts.Add(GetNYCContact(DotTypeFormContactTypes.Billing));
      request.DotTypeFormContacts.Add(GetNYCContact(DotTypeFormContactTypes.Technical));

      var response = (DotTypeFormsXmlResponseData) Engine.Engine.ProcessRequest(request, 689);

      var address1 = response.DotTypeFormsSchema.Form.FieldCollection[6];
      var address2 = response.DotTypeFormsSchema.Form.FieldCollection[7];

      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(address1.FieldRequired == "true");
      Assert.IsTrue(address2.FieldRequired == "false");
    }

    #region Contact Info Builders

    private DotTypeFormContact GetNYCContact(DotTypeFormContactTypes contactType)
    {
      var contact = new DotTypeFormContact(contactType, "Carina", "Shipley", "GoDaddy", "1 Parsons Drive", "",
        "NYC", "NY", "10007", "US", "2122943900", "2122943900", "cshipley@godaddy.com");
      return contact;
    }

    private DotTypeFormContact GetGeneralContact(DotTypeFormContactTypes contactType)
    {
      var contact = new DotTypeFormContact(contactType, "Raj", "Vontela", "GoDaddy", "123 Abc Rd", "Suite 45", 
        "Scottsdale", "AZ", "85260", "US", "4805058800", "4805058800", "rvontela@gd.com");
      return contact;
    }

    #endregion Contact Info Builders
  }
}
