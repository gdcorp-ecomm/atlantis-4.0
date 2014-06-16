using System;
using Atlantis.Framework.Basket.Impl;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Basket.Tests.IntegrationTests
{
  [TestClass]
  public class BasketServiceProxyTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DeleteItemsByPairWithTypeShouldNotThrowException()
    {
      const int lockcustomer = 1;
      const int lockmanager = 2;
      const int requestType = 836;
      
      var config = Engine.Engine.EngineRequestSettings.GetByRequestType(requestType);
      var wsConfig = config as WsConfigElement;
      if (wsConfig == null)
      {
        Assert.Inconclusive("Couldn't get config for requestType 836.");
      }

      var requestData = new BasketDeleteRequestData("1234", new[] {new BasketDeleteItemKey(1, 2)});
      int lockingMode = requestData.IsManager ? lockcustomer : lockmanager;
      new BasketServiceProxy().DeleteItemsByPairWithType(requestData.ShopperID, requestData.BasketType,
        requestData.GetItemKeysToDelete(), lockingMode, wsConfig.WSURL, (int)requestData.RequestTimeout.TotalMilliseconds);
    }
  }
}
