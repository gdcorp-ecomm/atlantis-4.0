using Atlantis.Framework.DotTypeEoi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeEoi.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeEoi.Impl.dll")]
  public class DotTypeGetGeneralEoiJsonTests
  {
    [TestMethod]
    public void DotTypeGetGeneralEoiJsonGoodRequest()
    {
      var request = new DotTypeGetGeneralEoiJsonRequestData();
      var response = (DotTypeGetGeneralEoiJsonResponseData)Engine.Engine.ProcessRequest(request, 698);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, response.DotTypeEoiResponse != null);
    }
  }
}
