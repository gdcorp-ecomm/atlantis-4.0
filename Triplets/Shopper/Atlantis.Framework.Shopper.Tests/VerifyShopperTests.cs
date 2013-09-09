using Atlantis.Framework.Shopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class VerifyShopperTests
  {
    [TestMethod]
    public void VerifyShopperRequestDataProperties()
    {
      var request = new VerifyShopperRequestData("blue");
      Assert.AreEqual("blue", request.ShopperID);
    }

    [TestMethod]
    public void VerifyShopperResponseDataPropertiesValid()
    {
      var response = VerifyShopperResponseData.FromPrivateLabelId(1724);
      Assert.AreEqual(1724, response.PrivateLabelId);
      Assert.IsTrue(response.IsVerified);
      string xml = response.ToXML();
      XElement.Parse(xml);
      Assert.IsNull(response.GetException());
    }

    [TestMethod]
    public void VerifyShopperResponseDataNotVerified()
    {
      Assert.AreEqual(0, VerifyShopperResponseData.NotVerified.PrivateLabelId);
      Assert.IsFalse(VerifyShopperResponseData.NotVerified.IsVerified);
    }

    [TestMethod]
    public void VerifyShopperResponseDataPropertiesInValidZero()
    {
      var response = VerifyShopperResponseData.FromPrivateLabelId(0);
      Assert.IsTrue(ReferenceEquals(response, VerifyShopperResponseData.NotVerified));
    }

    [TestMethod]
    public void VerifyShopperResponseDataPropertiesInValidNegative()
    {
      var response = VerifyShopperResponseData.FromPrivateLabelId(-44);
      Assert.IsTrue(ReferenceEquals(response, VerifyShopperResponseData.NotVerified));
    }

    private const int _REQUESTTYPE = 737;

    [TestMethod]
    public void VerifyShopperBasic()
    {
      var request = new VerifyShopperRequestData("832652");
      var response = (VerifyShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(1, response.PrivateLabelId);
    }

    [TestMethod]
    public void VerifyShopperEmpty()
    {
      var request = new VerifyShopperRequestData(string.Empty);
      var response = (VerifyShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(VerifyShopperResponseData.NotVerified, response));
    }

    [TestMethod]
    public void VerifyShopperNull()
    {
      var request = new VerifyShopperRequestData(null);
      var response = (VerifyShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(VerifyShopperResponseData.NotVerified, response));
    }

  }
}
