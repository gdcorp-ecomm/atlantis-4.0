using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Shopper.Interface;
using Atlantis.Framework.Shopper.Tests.Properties;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Tests
{
  [TestClass]
  public class VipShopperTests
  {
    [TestMethod]
    public void RequestDataProperties()
    {
      var request = new VipShopperRequestData("832652");
      Assert.AreEqual("832652", request.ShopperID);
      Assert.AreEqual("832652", request.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKey()
    {
      var request = new VipShopperRequestData("832652");
      var request2 = new VipShopperRequestData("mmo");
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataNullShopper()
    {
      var request = new VipShopperRequestData(null);
      Assert.AreEqual(string.Empty, request.ShopperID);
    }

    [TestMethod]
    public void ResponseDataVipShopperTrue()
    {
      string cacheXml = Resources.VipShopperExists;
      var response = VipShopperResponseData.FromCacheXml(cacheXml);
      Assert.IsTrue(response.IsVipShopper);
      Assert.AreNotEqual(string.Empty, response.RepFirstName);
      Assert.AreNotEqual(string.Empty, response.RepLastName);
      Assert.AreNotEqual(string.Empty, response.RepPhone);
      Assert.AreNotEqual(string.Empty, response.RepEmail);

      XElement.Parse(response.ToXML());
      Assert.IsNull(response.GetException());
    }

    [TestMethod]
    public void ResponseDataVipShopperFalse()
    {
      string cacheXml = Resources.VipShopperEmpty;
      var response = VipShopperResponseData.FromCacheXml(cacheXml);
      Assert.IsFalse(response.IsVipShopper);
      Assert.AreEqual(string.Empty, response.RepFirstName);
      Assert.AreEqual(string.Empty, response.RepLastName);
      Assert.AreEqual(string.Empty, response.RepPhone);
      Assert.AreEqual(string.Empty, response.RepEmail);

      Assert.IsTrue(ReferenceEquals(VipShopperResponseData.None, response));
    }

    [TestMethod]
    public void ResponseDataVipShopperMissingAttribute()
    {
      string cacheXml = Resources.VipShopperMissingAttributes;
      var response = VipShopperResponseData.FromCacheXml(cacheXml);
      Assert.IsTrue(response.IsVipShopper);
      Assert.AreEqual(string.Empty, response.RepPhone);
    }

    [TestMethod]
    public void ResponseDataVipShopperEmpty()
    {
      var response = VipShopperResponseData.FromCacheXml(string.Empty);
      Assert.IsFalse(response.IsVipShopper);
      Assert.IsTrue(ReferenceEquals(VipShopperResponseData.None, response));
    }

    [TestMethod]
    public void ResponseDataVipShopperNull()
    {
      var response = VipShopperResponseData.FromCacheXml(null);
      Assert.IsFalse(response.IsVipShopper);
      Assert.IsTrue(ReferenceEquals(VipShopperResponseData.None, response));
    }

    const int _REQUESTTYPE = 742;

    [TestMethod]
    public void VipShopperRequestBasic()
    {
      var request = new VipShopperRequestData("832652");
      var response = (VipShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(ReferenceEquals(VipShopperResponseData.None, response));
    }

    [TestMethod]
    public void VipShopperRequestEmptyShopper()
    {
      var request = new VipShopperRequestData(string.Empty);
      var response = (VipShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(VipShopperResponseData.None, response));
    }

  }
}
