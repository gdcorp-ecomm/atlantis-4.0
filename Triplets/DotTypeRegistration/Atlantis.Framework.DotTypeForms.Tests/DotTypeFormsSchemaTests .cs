using Atlantis.Framework.DotTypeForms.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeForms.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeForms.Impl.dll")]
  public class DotTypeFormsSchemaTests
  {
    [TestMethod]
    public void DotTypeFormsSchemaGoodRequest()
    {
      var request = new DotTypeFormsXmlSchemaRequestData(1640, "MOBILE", "GA", "EN");
      var response = (DotTypeFormsXmlSchemaResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeFormsSchemaBadRequest()
    {
      var request = new DotTypeFormsXmlSchemaRequestData(-1, "name of placement", "GA", "EN");
      var response = (DotTypeFormsXmlSchemaResponseData)Engine.Engine.ProcessRequest(request, 689);
      Assert.AreEqual(false, response.IsSuccess);
    }
  }
}
