using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeEoi.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeEoi.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeEoi.Impl.dll")]
  public class AddToShopperWatchListTests
  {
    [TestMethod]
    public void AddToShopperWatchListGoodRequest()
    {
      var gTlds = new List<IDotTypeEoiGtld>(1);
      var gTld = new DotTypeEoiGtld { Id = 1609, GtldSubCategoryId = 10320 };
      const string displayTime = "2013-05-23 10:29:55";
      gTlds.Add(gTld);
      var request = new AddToShopperWatchListRequestData("861126", displayTime, gTlds, "en");
      var response = (AddToShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, 704);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ResponseMessage));
    }

    [TestMethod]
    public void AddToShopperWatchListBadRequest()
    {
      try
      {
        var request = new AddToShopperWatchListRequestData("", "", new List<IDotTypeEoiGtld>(), "en");
      }
      catch (Exception)
      {
        Assert.AreEqual(1, 1);
      }
    }

    [TestMethod]
    public void AddToShopperWatchListRequestToXml()
    {
      var gTlds = new List<IDotTypeEoiGtld>(1);
      var gTld = new DotTypeEoiGtld { Id = 1609, GtldSubCategoryId = 10320 };
      const string displayTime = "2013-05-23 10:29:55";
      gTlds.Add(gTld);
      var request = new AddToShopperWatchListRequestData("", displayTime, gTlds, "en");
      Assert.AreEqual(true, !string.IsNullOrEmpty(request.ToXML()));
    }

    [TestMethod]
    public void AddToShopperWatchListResponseToXml()
    {
      var gTlds = new List<IDotTypeEoiGtld>(1);
      var gTld = new DotTypeEoiGtld { Id = 1609, GtldSubCategoryId = 10320 };
      const string displayTime = "2013-05-23 10:29:55";
      gTlds.Add(gTld);
      var request = new AddToShopperWatchListRequestData("", displayTime, gTlds, "en");
      var response = (AddToShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, 704);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

  }
}
