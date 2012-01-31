using System.Diagnostics;
using Atlantis.Framework.EcommHasLineOfCredit.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommHasLineOfCredit.Test
{
  [TestClass]
  public class UnitTest1
  {
    private const string _shopperId = "867900";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetShopperLineOfCredit()
    {
      var request = new EcommHasLineOfCreditRequestData(_shopperId, string.Empty, string.Empty, string.Empty, 0);
      var response = (EcommHasLineOfCreditResponseData)Engine.Engine.ProcessRequest(request, 482);

      if (response.IsSuccess)
      {
        Debug.WriteLine(response.HasLineOfCredit.ToString());
        Assert.IsNotNull(response.HasLineOfCredit.ToString());
      }
      else
      {
        Assert.Fail();
      }
    }
  }
}
