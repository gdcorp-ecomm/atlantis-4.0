using System.Diagnostics;
using Atlantis.Framework.EcommInstoreBalance.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommInstoreBalance.Test
{
  [TestClass]
  public class InstoreBalanceTests
  {
    private string _shopperId = "840748";  //no bal "867900";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void TestShopperHasBalance()
    {
      EcommInstoreBalanceRequestData request = new EcommInstoreBalanceRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0);
      EcommInstoreBalanceResponseData response = (EcommInstoreBalanceResponseData)Engine.Engine.ProcessRequest(request, 481);

      if (response.IsSuccess)
      {
        Debug.WriteLine(response.HasBalance.ToString());
       // Assert.IsFalse(response.HasBalance);
        Assert.IsTrue(response.HasBalance);
      }
      else
      {
        Assert.Fail();
      }
    }
  }
}
