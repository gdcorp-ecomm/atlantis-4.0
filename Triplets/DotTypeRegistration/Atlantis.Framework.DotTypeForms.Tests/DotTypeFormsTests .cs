using Atlantis.Framework.DotTypeForms.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;

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
    public void DotTypeFormsXml_Constructor()
    {
      var dotTypeFormsSchema = new DotTypeFormsSchema();
      Assert.IsInstanceOfType(dotTypeFormsSchema, typeof(DotTypeFormsSchema));
    }

    #region DotTypeFormsXmlRequestData

    [TestMethod]
    public void DotTypeFormsXmlRequestData_ToXml()
    {
      var request = new DotTypeFormsXmlRequestData("dpp", 71, "mobile", "GA", "EN-US", 1)
      {
        DotTypeFormContacts = new List<DotTypeFormContact>(4),
        RequestTimeout = TimeSpan.FromSeconds(15)
      };

      request.DotTypeFormContacts.Add(new DotTypeFormContact(DotTypeFormContactTypes.Registrant, "Raj", "Vontela", "GoDaddy", "123 Abc Rd", "Suite 45", "Scottsdale", "AZ", "85260", "US", "4805058800", "4805058800", "rvontela@gd.com"));
      request.DotTypeFormContacts.Add(new DotTypeFormContact(DotTypeFormContactTypes.Administrative, "Raj", "Vontela", "GoDaddy", "123 Abc Rd", "Suite 45", "Scottsdale", "AZ", "85260", "US", "4805058800", "4805058800", "rvontela@gd.com"));
      request.DotTypeFormContacts.Add(new DotTypeFormContact(DotTypeFormContactTypes.Billing, "Raj", "Vontela", "GoDaddy", "123 Abc Rd", "Suite 45", "Scottsdale", "AZ", "85260", "US", "4805058800", "4805058800", "rvontela@gd.com"));
      request.DotTypeFormContacts.Add(new DotTypeFormContact(DotTypeFormContactTypes.Technical, "Raj", "Vontela", "GoDaddy", "123 Abc Rd", "Suite 45", "Scottsdale", "AZ", "85260", "US", "4805058800", "4805058800", "rvontela@gd.com"));


      var xml = request.ToXML();

      var expected = "<parameters formtype=\"dpp\" tldid=\"71\" placement=\"mobile\" phase=\"ga\" marketId=\"en-us\" contextid=\"1\">\r\n";
      expected += "  <contacts>\r\n";
      expected += "    <contact contacttype=\"Registrant\" firstname=\"Raj\" lastname=\"Vontela\" company=\"GoDaddy\" address1=\"123 Abc Rd\" address2=\"Suite 45\" city=\"Scottsdale\" state=\"AZ\" zip=\"85260\" country=\"US\" phone=\"4805058800\" fax=\"4805058800\" email=\"rvontela@gd.com\" />\r\n";
      expected += "    <contact contacttype=\"Administrative\" firstname=\"Raj\" lastname=\"Vontela\" company=\"GoDaddy\" address1=\"123 Abc Rd\" address2=\"Suite 45\" city=\"Scottsdale\" state=\"AZ\" zip=\"85260\" country=\"US\" phone=\"4805058800\" fax=\"4805058800\" email=\"rvontela@gd.com\" />\r\n";
      expected += "    <contact contacttype=\"Billing\" firstname=\"Raj\" lastname=\"Vontela\" company=\"GoDaddy\" address1=\"123 Abc Rd\" address2=\"Suite 45\" city=\"Scottsdale\" state=\"AZ\" zip=\"85260\" country=\"US\" phone=\"4805058800\" fax=\"4805058800\" email=\"rvontela@gd.com\" />\r\n";
      expected += "    <contact contacttype=\"Technical\" firstname=\"Raj\" lastname=\"Vontela\" company=\"GoDaddy\" address1=\"123 Abc Rd\" address2=\"Suite 45\" city=\"Scottsdale\" state=\"AZ\" zip=\"85260\" country=\"US\" phone=\"4805058800\" fax=\"4805058800\" email=\"rvontela@gd.com\" />\r\n";
      expected += "  </contacts>\r\n";
      expected += "</parameters>";
      
      Assert.AreEqual(expected, xml);
    }

    [TestMethod]
    public void DotTypeFormsXmlRequestData_GetCacheMD5()
    {
      var request = new DotTypeFormsXmlRequestData("dpp", 2021, "fos", "lr", "EN-US", 1);
      var hash = request.GetCacheMD5();
      const string expected = "4D-E3-67-75-B7-FC-C2-F5-E9-82-89-4B-A6-66-D1-21";
      Assert.AreEqual(expected, hash);
    }

    #endregion DotTypeFormsXmlRequestData

    #region DotTypeFormsHtmlRequestData

    [TestMethod]
    public void DotTypeFormsHtmlRequestData_ToXml()
    {
      var request = new DotTypeFormsHtmlRequestData("trademark", 1731, "FOS", "SRA", "EN-US", 1, "fhdsjkaflhdskjaflsa.sunrise");
      var xml = request.ToXML();
      const string expected = "<parameters formtype=\"trademark\" tldid=\"1731\" placement=\"fos\" phase=\"sra\" marketId=\"en-us\" contextid=\"1\" domain=\"fhdsjkaflhdskjaflsa.sunrise\" />";
      Assert.AreEqual(expected, xml);
    }

    [TestMethod]
    public void DotTypeFormsHtmlRequestData_GetCacheMD5()
    {
      var request = new DotTypeFormsHtmlRequestData("trademark", 1731, "FOS", "SRA", "EN-US", 1, "fhdsjkaflhdskjaflsa.sunrise");
      var hash = request.GetCacheMD5();
      const string expected = "1D-93-20-E1-9D-BF-BF-49-9D-1F-71-2F-B0-8C-85-79";
      Assert.AreEqual(expected, hash);
    }

    #endregion DotTypeFormsHtmlRequestData

    #region Integration Tests

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

      request.DotTypeFormContacts.Add(GetDKContact(DotTypeFormContactTypes.Registrant));
      request.DotTypeFormContacts.Add(GetDKContact(DotTypeFormContactTypes.Administrative));
      request.DotTypeFormContacts.Add(GetDKContact(DotTypeFormContactTypes.Billing));
      request.DotTypeFormContacts.Add(GetDKContact(DotTypeFormContactTypes.Technical));

      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(!string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeFormsXmlPostGoodRequestForNycEligibility()
    {
      // TldId = 2021 is .NYC
      var request = new DotTypeFormsXmlRequestData("dpp", 2021, "fos", "lr", "EN-US", 1)
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
    public void DotTypeFormsXml_PostGoodRequestForLawyerEligibility()
    {
      // TldId = 2404 is .LAWYER
      var request = new DotTypeFormsXmlRequestData("dpp", 2404, "fos", "lr", "EN-US", 1)
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
      Assert.IsTrue(response.ToXML().Contains("validationrule name=\"RLawyerRegulatoryBody\""));
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

      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);

      Assert.AreEqual(true, response.IsSuccess);

      foreach (IDotTypeFormsField field in response.DotTypeFormsSchema.Form.FieldCollection)
      {
        if (field.FieldName == "address1")
        {
          Assert.IsTrue(field.FieldRequired == "true");
        }

        if (field.FieldName == "fax")
        {
          Assert.IsTrue(field.FieldRequired == "false");
        }
      }
    }

    [TestMethod]
    public void DotTypeFormsToJsonContainsDefaultValueFlag()
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

      foreach (IDotTypeFormsField field in response.DotTypeFormsSchema.Form.FieldCollection)
      {
        if (field.FieldName == "address1")
        {
          Assert.IsTrue(field.FieldDefaultValue.Trim() == "2 Broadway Suite 4");
        }
      }
    }

    [TestMethod]
    public void TuiResponseSuccessfulButNoContent()
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
      Assert.IsTrue(response.TuiResponseStatusCode == HttpStatusCode.NoContent);
    }

    [TestMethod]
    public void TuiResponseSuccessfulWithContent()
    {
      // TldId = 71 is .DK
      // dpp = eligibility
      var request = new DotTypeFormsXmlRequestData("dpp", 71, "MOBILE", "GA", "EN-US", 1)
      {
        DotTypeFormContacts = new List<DotTypeFormContact>(4),
        RequestTimeout = TimeSpan.FromSeconds(15)
      };

      request.DotTypeFormContacts.Add(GetDKContact(DotTypeFormContactTypes.Registrant));
      request.DotTypeFormContacts.Add(GetDKContact(DotTypeFormContactTypes.Administrative));
      request.DotTypeFormContacts.Add(GetDKContact(DotTypeFormContactTypes.Billing));
      request.DotTypeFormContacts.Add(GetDKContact(DotTypeFormContactTypes.Technical));

      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.IsTrue(response.TuiResponseStatusCode == HttpStatusCode.OK);
      Assert.IsTrue(!string.IsNullOrEmpty(response.ToXML()));
    }

    #endregion Integration Tests

    #region Contact Info Builders

    private DotTypeFormContact GetNYCContact(DotTypeFormContactTypes contactType)
    {
      var contact = new DotTypeFormContact(contactType, "Carina", "Shipley", "GoDaddy", "2 Broadway", "Suite 4",
        "New York", "NY", "10004", "US", "2122943900", "2122943900", "cshipley@godaddy.com");
      return contact;
    }

    private DotTypeFormContact GetDKContact(DotTypeFormContactTypes contactType)
    {
      var contact = new DotTypeFormContact(contactType, "Raj", "Vontela", "GoDaddy", "123 Abc Rd", "Suite 45",
        "Scottsdale", "AZ", "85260", "DK", "4805058800", "4805058800", "rvontela@gd.com");
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
