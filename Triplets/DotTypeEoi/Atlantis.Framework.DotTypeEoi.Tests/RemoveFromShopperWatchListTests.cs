using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeEoi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeEoi.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeEoi.Impl.dll")]
  public class RemoveFromShopperWatchListTests
  {
    [TestMethod]
    public void RemoveFromShopperWatchListGoodRequest()
    {
      var gTlds = new List<string> { "18" };
      var request = new RemoveFromShopperWatchListRequestData("861126", gTlds);
      var response = (RemoveFromShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, 705);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ResponseMessage));
    }

    [TestMethod]
    public void RemoveFromShopperWatchListBadRequest()
    {
      try
      {
        var request = new RemoveFromShopperWatchListRequestData("861126", new List<string>());
      }
      catch (Exception)
      {
        Assert.AreEqual(1, 1);
      }
    }

    [TestMethod]
    public void RemoveFromShopperWatchListRequestToXml()
    {
      var gTlds = new List<string> { "18" };
      var request = new RemoveFromShopperWatchListRequestData("861126", gTlds);
      Assert.AreEqual(true, !string.IsNullOrEmpty(request.ToXML()));
    }

    [TestMethod]
    public void RemoveFromShopperWatchListResponseToXml()
    {
      var gTlds = new List<string> { "18" };
      var request = new RemoveFromShopperWatchListRequestData("861126", gTlds);
      var response = (RemoveFromShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, 705);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }
  }
}
