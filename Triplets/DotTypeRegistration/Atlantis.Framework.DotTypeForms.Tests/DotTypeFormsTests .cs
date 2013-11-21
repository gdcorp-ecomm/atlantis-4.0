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
      var request = new DotTypeFormsXmlRequestData("dpp", 1677, "MOBILE", "GA", "EN-US", 1);
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
    public void DotTypeFormsHtmlGoodRequest()
    {
      var request = new DotTypeFormsHtmlRequestData("dpp", 1677, "FOS", "GA", "EN-US", 1, string.Empty);
      var response = (DotTypeFormsHtmlResponseData)Engine.Engine.ProcessRequest(request, 709);
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
