using Atlantis.Framework.Engine;
using Atlantis.Framework.MYAGetExpiringProductsDetail.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MYAGetExpiringProductsDetail.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.MYAGetExpiringProductsDetail.Impl.dll")]
  public class MYAGetExpiringProductsDetailTests
  {
    [TestMethod]
    public void MYAGetExpiringProductsDetail()
    {
      //returns all products for shopper
      var requestData = new MYAGetExpiringProductsDetailRequestData("822497");     
      
      var response = (MYAGetExpiringProductsDetailResponseData)Engine.ProcessRequest(requestData, 194);
      
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void MYAGetExpiringProductsDetailWithAutoRenew()
    {
      //returns all products for shopper
      var requestData = new MYAGetExpiringProductsDetailRequestData("822497", true);

      var response = (MYAGetExpiringProductsDetailResponseData)Engine.ProcessRequest(requestData, 194);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void MYASpecificExpiringProductsDetail()
    {
      var requestData = new MYAGetExpiringProductsDetailRequestData("822497");
      requestData.ProductTypeHashSet.Add("14");

      var response = (MYAGetExpiringProductsDetailResponseData)Engine.ProcessRequest(requestData, 194);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void MYASpecificExpiringProductsDetailMoreThanFiveProductTypes()
    {
      var requestData = new MYAGetExpiringProductsDetailRequestData("822497");
      requestData.ProductTypeHashSet.Add("14");
      requestData.ProductTypeHashSet.Add("15");
      requestData.ProductTypeHashSet.Add("16");
      requestData.ProductTypeHashSet.Add("17");
      requestData.ProductTypeHashSet.Add("18");
      requestData.ProductTypeHashSet.Add("20");

      var response = (MYAGetExpiringProductsDetailResponseData)Engine.ProcessRequest(requestData, 194);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
