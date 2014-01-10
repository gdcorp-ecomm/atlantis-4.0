using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.DotTypeValidation.Impl;
using Atlantis.Framework.DotTypeValidation.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeValidation.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeValidation.Impl.dll")]
  public class DotTypeValidationTests
  {
    [TestMethod]
    public void DotTypeValidationEligibilityGoodRequest()
    {
      var fields = new Dictionary<string, IDotTypeValidationFieldValueData>();
      fields["clrut"] = DotTypeValidationFieldValueData.Create("56842879K");

      var request = new DotTypeValidationRequestData("dpp_absol", "d1wsdv", 59, "GA", "apptoken", fields);
      var response = (DotTypeValidationResponseData)Engine.Engine.ProcessRequest(request, 695);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));      
    }

    [TestMethod]
    public void DotTypeValidationBadRequest()
    {
      var fields = new Dictionary<string, IDotTypeValidationFieldValueData>();
      fields["clrut"] = DotTypeValidationFieldValueData.Create("CCO-AVCSD");

      var request = new DotTypeValidationRequestData("", "", 1577, "", "apptokenzsxs", fields);
      var response = (DotTypeValidationResponseData)Engine.Engine.ProcessRequest(request, 695);
      Assert.AreEqual(true, !response.IsSuccess);
      Assert.AreEqual(true, response.ToXML().Contains("error"));
    }

    [TestMethod]
    public void DotTypeValidationClaimXmlGoodRequest()
    {
      var fields = new Dictionary<string, IDotTypeValidationFieldValueData>();
      const string claimsHtml = @"<test></test>";
      fields["claims"] = DotTypeValidationFieldValueData.Create(claimsHtml);
      fields["acceptedDate"] = DotTypeValidationFieldValueData.Create(DateTime.Now.ToUniversalTime().ToString("o"));

      var request = new DotTypeValidationRequestData("dpp_absol", "d1wsdv", 1764, "LR", "apptoken", fields);
      var response = (DotTypeValidationResponseData)Engine.Engine.ProcessRequest(request, 695);
      //Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeValidationClaimXmlGoodRequestWithError()
    {
      var fields = new Dictionary<string, IDotTypeValidationFieldValueData>();
      const string claimsHtml = @"<test></test>";
      fields["claims"] = DotTypeValidationFieldValueData.Create(claimsHtml);
      fields["acceptedDate"] = DotTypeValidationFieldValueData.Create(DateTime.Now.ToUniversalTime().ToString("o"), false);

      var request = new DotTypeValidationRequestData("dpp_absol", "d1wsdv", 1764, "LR", "apptoken", fields);
      var response = (DotTypeValidationResponseData)Engine.Engine.ProcessRequest(request, 695);
      //Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }
  }
}
