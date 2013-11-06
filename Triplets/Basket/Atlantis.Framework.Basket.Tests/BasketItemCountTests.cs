using Atlantis.Framework.Basket.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Basket.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Basket.Impl.dll")]
  public class BasketItemCountTests
  {
    [TestMethod]
    public void BasketItemCountRequestDataProperties()
    {
      var request = new BasketItemCountRequestData("832652");
      Assert.AreEqual("832652", request.ShopperID);
      Assert.AreEqual(2d, request.RequestTimeout.TotalSeconds);
    }

    [TestMethod]
    public void BasketItemCountRequestDataPropertiesEmptyShopper()
    {
      var request = new BasketItemCountRequestData(string.Empty);
      Assert.AreEqual(string.Empty, request.ShopperID);
    }

    [TestMethod]
    public void BasketItemCountRequestDataPropertiesNullShopper()
    {
      var request = new BasketItemCountRequestData(null);
      Assert.AreEqual(null, request.ShopperID);
    }

    [TestMethod]
    public void BasketItemCountResponseDataProperties()
    {
      var response = BasketItemCountResponseData.FromPipeDelimitedResponseString("4|2");
      Assert.AreEqual(4, response.TotalItems);
    }

    [TestMethod]
    public void BasketItemCountResponseDataPropertiesNoMarketCount()
    {
      var response = BasketItemCountResponseData.FromPipeDelimitedResponseString("4");
      Assert.AreEqual(4, response.TotalItems);
    }

    [TestMethod]
    public void BasketItemCountResponseDataPropertiesNull()
    {
      var response = BasketItemCountResponseData.FromPipeDelimitedResponseString(null);
      Assert.AreEqual(0, response.TotalItems);
      Assert.IsTrue(ReferenceEquals(BasketItemCountResponseData.Empty, response));
    }

    [TestMethod]
    public void BasketItemCountResponseDataPropertiesEmpty()
    {
      var response = BasketItemCountResponseData.FromPipeDelimitedResponseString(string.Empty);
      Assert.AreEqual(0, response.TotalItems);
      Assert.IsTrue(ReferenceEquals(BasketItemCountResponseData.Empty, response));
    }

    [TestMethod]
    public void BasketItemCountResponseDataPropertiesZero()
    {
      var response = BasketItemCountResponseData.FromPipeDelimitedResponseString("0");
      Assert.AreEqual(0, response.TotalItems);
      Assert.IsTrue(ReferenceEquals(BasketItemCountResponseData.Empty, response));
    }

    [TestMethod]
    public void BasketItemCountResponseDataPropertiesNonNumber()
    {
      var response = BasketItemCountResponseData.FromPipeDelimitedResponseString("garbage");
      Assert.AreEqual(0, response.TotalItems);
      Assert.IsTrue(ReferenceEquals(BasketItemCountResponseData.Empty, response));
    }

    [TestMethod]
    public void BasketItemCountResponseDataPropertiesPipeOnly()
    {
      var response = BasketItemCountResponseData.FromPipeDelimitedResponseString("|");
      Assert.AreEqual(0, response.TotalItems);
      Assert.IsTrue(ReferenceEquals(BasketItemCountResponseData.Empty, response));
    }

    private const int _REQUESTTYPE = 760;

    [TestMethod]
    public void BasketItemRequestNullShopper()
    {
      var request = new BasketItemCountRequestData(null);
      var response = (BasketItemCountResponseData) Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, response.TotalItems);
      Assert.IsTrue(ReferenceEquals(BasketItemCountResponseData.Empty, response));
    }

    [TestMethod]
    public void BasketItemRequestEmptyShopper()
    {
      var request = new BasketItemCountRequestData(string.Empty);
      var response = (BasketItemCountResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, response.TotalItems);
      Assert.IsTrue(ReferenceEquals(BasketItemCountResponseData.Empty, response));
    }

    [TestMethod]
    public void BasketItemRequestBadShopper()
    {
      var request = new BasketItemCountRequestData("garbage");
      var response = (BasketItemCountResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, response.TotalItems);
      Assert.IsTrue(ReferenceEquals(BasketItemCountResponseData.Empty, response));
    }

    [TestMethod]
    public void BasketItemRequestValidShopper()
    {
      var request = new BasketItemCountRequestData("832652");
      var response = (BasketItemCountResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);

      if (response.TotalItems > 0)
      {
        Assert.IsFalse(ReferenceEquals(BasketItemCountResponseData.Empty, response));
      }
    }



  }
}
