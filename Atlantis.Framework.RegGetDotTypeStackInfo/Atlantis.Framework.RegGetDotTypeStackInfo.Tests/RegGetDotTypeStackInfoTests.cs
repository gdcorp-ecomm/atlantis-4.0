using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RegGetDotTypeStackInfo.Interface;

namespace Atlantis.Framework.RegGetDotTypeStackInfo.Tests
{
  [TestClass]
  public class RegGetDotTypeStackInfoTests
  {

    [TestInitialize]
    public void InitializeTests()
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      ((TestContexts)siteContext).SetContextInfo(1, "832652");
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)shopperContext).SetContextInfo(1, "832652");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPriceForTLD()
    {
      RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 1, "USD");
      RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)DataCache.DataCache.GetProcessRequest(req, 349);

      Assert.IsNotNull(resp);
      Assert.AreNotEqual<int>(0, resp.GetPriceForTld("COM", "reoffer", true));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPriceForTLDEuro()
    {
        RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 1, "EUR");
        RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)DataCache.DataCache.GetProcessRequest(req, 340);

        Assert.IsNotNull(resp);
        Assert.AreNotEqual<int>(0, resp.GetPriceForTld("COM", "reoffer", true));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPriceForTLD_DoesntExist()
    {
      RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 1, "USD");
      RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)DataCache.DataCache.GetProcessRequest(req, 349);

      Assert.IsNotNull(resp);
      Assert.AreEqual<int>(0, resp.GetPriceForTld("zsz", "reoffer99", true));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetStackIdForTLD()
    {
      RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 1, "USD");
      RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)DataCache.DataCache.GetProcessRequest(req, 349);

      Assert.IsNotNull(resp);
      Assert.AreNotEqual<int>(0, resp.GetStackIdForTld("COM", "reoffer"));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetStackCount()
    {
      RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 1, "USD");
      RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)DataCache.DataCache.GetProcessRequest(req, 349);

      Assert.IsNotNull(resp);
      Assert.AreNotEqual<int>(0, resp.Count);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetEmptyCacheWithBadInput()
    {
      RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 99, "ZZZ");
      RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)DataCache.DataCache.GetProcessRequest(req, 349);

      Assert.IsNotNull(resp);
      Assert.AreEqual<int>(0, resp.Count);
    }

  }
}
