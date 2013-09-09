using Atlantis.Framework.Shopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class GetShopperResponseCacheTests
  {
    [TestMethod]
    public void EmptyCache()
    {
      var cache = new GetShopperResponseCache();
      string value;
      bool success = cache.TryGetShopperData("832652", "city", out value);
      Assert.IsFalse(success);
      Assert.AreEqual(string.Empty, value);
    }

    [TestMethod]
    public void EmptyCacheEmptyShopper()
    {
      var cache = new GetShopperResponseCache();
      string value;
      bool success = cache.TryGetShopperData(string.Empty, "city", out value);
      Assert.IsTrue(success);
      Assert.AreEqual(string.Empty, value);
    }

    [TestMethod]
    public void EmptyCacheNullShopper()
    {
      var cache = new GetShopperResponseCache();
      string value;
      bool success = cache.TryGetShopperData(null, "city", out value);
      Assert.IsTrue(success);
      Assert.AreEqual(string.Empty, value);
    }

    private const string _SHOPPEREMPTY_RESPONSE_XML = "<Shopper ID=\"\"><Fields><Field Name=\"first_name\">Mr.</Field></Fields><Field Name=\"last_name\">Bojangles</Field></Shopper>";

    [TestMethod]
    public void UpdateCacheWithEmptyShopperResponse()
    {
      var cache = new GetShopperResponseCache();
      var getShopperResponse = GetShopperResponseData.FromShopperXml(_SHOPPEREMPTY_RESPONSE_XML);
      cache.CacheShopperData(getShopperResponse);

      string value;
      bool success = cache.TryGetShopperData(string.Empty, "first_name", out value);
      Assert.IsTrue(success);
      Assert.AreEqual(string.Empty, value);
    }

    private const string _SHOPPER_RESPONSE_XML = "<Shopper ID=\"822497\"><Fields><Field Name=\"first_name\">Mr.</Field></Fields><Field Name=\"last_name\">Bojangles</Field></Shopper>";

    [TestMethod]
    public void UpdateCacheWithValidShopperResponse()
    {
      var cache = new GetShopperResponseCache();
      var getShopperResponse = GetShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_XML);
      cache.CacheShopperData(getShopperResponse);

      string value;
      bool success = cache.TryGetShopperData("822497", "first_name", out value);
      Assert.IsTrue(success);
      Assert.AreEqual("Mr.", value);

      success = cache.TryGetShopperData("832652", "first_name", out value);
      Assert.IsFalse(success);
      Assert.AreEqual(string.Empty, value);
    }

    private const string _SHOPPER_RESPONSE_FIRSTNAME = "<Shopper ID=\"822497\"><Fields><Field Name=\"first_name\">Mr.</Field></Fields></Shopper>";
    private const string _SHOPPER_RESPONSE_SECONDNAME = "<Shopper ID=\"822497\"><Fields><Field Name=\"last_name\">Bojangles</Field></Fields></Shopper>";

    [TestMethod]
    public void UpdateCacheWithValidShopperResponses()
    {
      var cache = new GetShopperResponseCache();
      var getShopperResponse = GetShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_FIRSTNAME);
      cache.CacheShopperData(getShopperResponse);

      getShopperResponse = GetShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_SECONDNAME);
      cache.CacheShopperData(getShopperResponse);

      string value;
      bool success = cache.TryGetShopperData("822497", "last_name", out value);
      Assert.IsTrue(success);
      Assert.AreEqual("Bojangles", value);
    }

  }
}
