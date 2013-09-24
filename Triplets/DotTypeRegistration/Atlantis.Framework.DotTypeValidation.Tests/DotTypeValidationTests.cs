using System.Collections.Generic;
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
    public void DotTypeValidationGoodRequest()
    {
      var fields = new Dictionary<string, string>();
      fields["legaltype"] = "CCO";
      var request = new DotTypeValidationRequestData("dpp_absol", "d1wsdv", 1577, "LR", "apptoken", fields);
      var response = (DotTypeValidationResponseData)Engine.Engine.ProcessRequest(request, 695);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));      
    }

    [TestMethod]
    public void DotTypeValidationBadRequest()
    {
      var fields = new Dictionary<string, string>();
      fields["legaltype"] = "CCO-AVCSD";
      var request = new DotTypeValidationRequestData("", "", 1577, "", "apptokenzsxs", fields);
      var response = (DotTypeValidationResponseData)Engine.Engine.ProcessRequest(request, 695);
      Assert.AreEqual(true, !response.IsSuccess);
      Assert.AreEqual(true, response.ToXML().Contains("error"));
    }
  }
}
