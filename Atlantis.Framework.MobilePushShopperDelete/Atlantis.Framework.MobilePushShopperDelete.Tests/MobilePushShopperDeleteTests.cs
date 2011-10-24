using System;
using System.Diagnostics;
using Atlantis.Framework.MobilePushShopperDelete.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MobilePushShopperDelete.Tests
{
  [TestClass]
  public class MobilePushShopperDeleteTests
  {
    private void WriteMessageToConsole(string message)
    {
      Console.WriteLine(message);
      Debug.WriteLine(message);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ValidShopperDeleteNotification()
    {
      int shopperPushId = 14;

      MobilePushShopperDeleteRequestData requestData = new MobilePushShopperDeleteRequestData(shopperPushId,
                                                                                              "840820",
                                                                                              "http://www.MobilePushShopperAddTests.com",
                                                                                              string.Empty,
                                                                                              Guid.NewGuid().ToString(),
                                                                                              1);

      try
      {
        MobilePushShopperDeleteResponseData responseData = (MobilePushShopperDeleteResponseData)Engine.Engine.ProcessRequest(requestData, 431);
        WriteMessageToConsole(responseData.Xml);
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        WriteMessageToConsole(ex.Message + " " + ex.StackTrace);
        Assert.Fail();
      }
    }
  }
}
