using Atlantis.Framework.DotTypeEoi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeEoi.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeEoi.Impl.dll")]
  public class DotTypeGeneralEoiJsonTests
  {
    [TestMethod]
    public void DotTypeGetGeneralEoiJsonGoodRequest()
    {
      var request = new GeneralEoiJsonRequestData("en");
      var response = (GeneralEoiJsonResponseData)Engine.Engine.ProcessRequest(request, 698);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, response.DotTypeEoiResponse != null);
      Assert.AreEqual(true, response.DotTypeEoiResponse.Categories.Count > 0);
      Assert.AreEqual(true, response.DotTypeEoiResponse.Categories[0].SubCategories.Count > 0);
      Assert.AreEqual(true, response.DotTypeEoiResponse.Categories[0].SubCategories[0].SubCategoryId > -1);
      Assert.AreEqual(true, response.DotTypeEoiResponse.Categories[0].SubCategories[0].Gtlds.Count > 0);
      Assert.AreEqual(true, response.DotTypeEoiResponse.Categories[0].SubCategories[0].Gtlds[0].Id > -1);

      Assert.AreEqual(true, response.DotTypeEoiResponse.Categories[0].Gtlds.Count > 0);
      Assert.AreEqual(true, response.DotTypeEoiResponse.Categories[0].Gtlds[0].Id > -1);
    }
  }
}
