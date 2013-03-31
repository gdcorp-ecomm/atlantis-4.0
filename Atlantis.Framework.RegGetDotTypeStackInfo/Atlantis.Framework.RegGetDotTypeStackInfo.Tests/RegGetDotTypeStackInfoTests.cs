using Atlantis.Framework.RegGetDotTypeStackInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RegGetDotTypeStackInfo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.RegGetDotTypeStackInfo.Impl.dll")]
  public class RegGetDotTypeStackInfoTests
  {
    [TestMethod]
    public void GetPriceForTLD()
    {
      RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 1, "USD");
      RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)Engine.Engine.ProcessRequest(req, 349);

      Assert.IsNotNull(resp);
      Assert.AreNotEqual<int>(0, resp.GetPriceForTld("COM", "reoffer", true));
    }

    [TestMethod]
    public void GetPriceForTLDEuro()
    {
        RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 1, "EUR");
        RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)Engine.Engine.ProcessRequest(req, 349);

        Assert.IsNotNull(resp);
        Assert.AreNotEqual<int>(0, resp.GetPriceForTld("COM", "reoffer", true));
    }

    [TestMethod]
    public void GetPriceForTLD_DoesntExist()
    {
      RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 1, "USD");
      RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)Engine.Engine.ProcessRequest(req, 349);

      Assert.IsNotNull(resp);
      Assert.AreEqual<int>(0, resp.GetPriceForTld("zsz", "reoffer99", true));
    }

    [TestMethod]
    public void GetStackIdForTLD()
    {
      RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 1, "USD");
      RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)Engine.Engine.ProcessRequest(req, 349);

      Assert.IsNotNull(resp);
      Assert.AreNotEqual<int>(0, resp.GetStackIdForTld("COM", "reoffer"));
    }

    [TestMethod]
    public void GetStackCount()
    {
      RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 1, "USD");
      RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)Engine.Engine.ProcessRequest(req, 349);

      Assert.IsNotNull(resp);
      Assert.AreNotEqual<int>(0, resp.Count);
    }

    [TestMethod]
    public void GetEmptyCacheWithBadInput()
    {
      RegGetDotTypeStackInfoRequestData req = new RegGetDotTypeStackInfoRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, 99, "ZZZ");
      RegGetDotTypeStackInfoResponseData resp = (RegGetDotTypeStackInfoResponseData)Engine.Engine.ProcessRequest(req, 349);

      Assert.IsNotNull(resp);
      Assert.AreEqual<int>(0, resp.Count);
    }

  }
}
