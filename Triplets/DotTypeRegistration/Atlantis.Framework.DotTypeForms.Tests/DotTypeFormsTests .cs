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
      var request = new DotTypeFormsXmlRequestData(1640, "MOBILE", "GA", "EN");
      var response = (DotTypeFormsXmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeFormsXmlBadRequest()
    {
      var request = new DotTypeFormsXmlRequestData(-1, "name of placement", "GA", "EN");
      var response = (DotTypeFormsHtmlResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(false, response.IsSuccess);
    }

    [TestMethod]
    public void DotTypeFormsHtmlGoodRequest()
    {
      var request = new DotTypeFormsHtmlRequestData(1640, "MOBILE", "GA", "EN");
      var response = (DotTypeFormsHtmlResponseData)Engine.Engine.ProcessRequest(request, 709);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeFormsHtmlBadRequest()
    {
      var request = new DotTypeFormsHtmlRequestData(-1, "name of placement", "GA", "EN");
      var response = (DotTypeFormsHtmlResponseData)Engine.Engine.ProcessRequest(request, 709);
      Assert.AreEqual(false, response.IsSuccess);
    }
  }
}
