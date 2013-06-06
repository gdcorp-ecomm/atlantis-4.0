﻿using System.Collections.Generic;
using Atlantis.Framework.DotTypeEoi.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeEoi.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeEoi.Impl.dll")]
  public class ShopperWatchListJsonTests
  {
    [TestMethod]
    public void ShopperWatchListJsonSuccess()
    {
      var gTlds = new List<IDotTypeEoiGtld>(1);
      var gTld = new DotTypeEoiGtld { Id = 42, GtldSubCategoryId = 2952 };
      const string displayTime = "2013-05-23 10:29:55";
      gTlds.Add(gTld);
      var request1 = new AddToShopperWatchListRequestData("861126", displayTime, gTlds, "en-us");
      Engine.Engine.ProcessRequest(request1, 704);
      var request2 = new ShopperWatchListJsonRequestData("861126", "en-us");
      var response2 = (ShopperWatchListResponseData)Engine.Engine.ProcessRequest(request2, 703);
      Assert.AreEqual(true, response2.IsSuccess);
      Assert.AreEqual(true, response2.ShopperWatchListResponse != null);
      Assert.AreEqual(true, response2.ShopperWatchListResponse.GtldIdDictionary.Count > 0);
      Assert.AreEqual(true, response2.ShopperWatchListResponse.GtldIdDictionary[42].Id > -1);
    }
  }
}
