using Atlantis.Framework.Engine;
using Atlantis.Framework.MYAGetExpiringProductsDetail.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MYAGetExpiringProductsDetail.Tests
{
  [TestClass]
  public class MYAGetExpiringProductsDetailTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MYAGetExpiringProductsDetail()
    {
      //returns all products for shopper
      MYAGetExpiringProductsDetailRequestData requestData = new MYAGetExpiringProductsDetailRequestData(
        "822497", string.Empty, string.Empty, string.Empty, 0);     
      MYAGetExpiringProductsDetailResponseData response = (MYAGetExpiringProductsDetailResponseData)Engine.ProcessRequest(requestData, 194);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MYASpecificExpiringProductsDetail()
    {
      MYAGetExpiringProductsDetailRequestData requestData = new MYAGetExpiringProductsDetailRequestData(
        "822497", string.Empty, string.Empty, string.Empty, 0);
      requestData.ProductTypeIdList = "14";
      MYAGetExpiringProductsDetailResponseData response = (MYAGetExpiringProductsDetailResponseData)Engine.ProcessRequest(requestData, 194);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
