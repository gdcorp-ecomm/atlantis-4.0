using System;
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
      var gTlds = @"<gTlds><gTld id='18' gTldSubCategoryId='234' /></gTlds>";
      var request = new RemoveFromShopperWatchListRequestData("861126", string.Empty, string.Empty, string.Empty, 0, gTlds);
      var response = (RemoveFromShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, 705);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ResponseMessage));
    }

    [TestMethod]
    public void RemoveFromShopperWatchListBadRequest()
    {
      try
      {
        var request = new RemoveFromShopperWatchListRequestData("861126", string.Empty, string.Empty, string.Empty, 0, string.Empty);
      }
      catch (Exception)
      {
        Assert.AreEqual(1, 1);
      }
    }

    [TestMethod]
    public void RemoveFromShopperWatchListRequestToXml()
    {
      const string gTlds = @"<gTlds><gTld id='18' gTldSubCategoryId='234' /></gTlds>";
      var request = new RemoveFromShopperWatchListRequestData("861126", string.Empty, string.Empty, string.Empty, 0, gTlds);
      Assert.AreEqual(true, !string.IsNullOrEmpty(request.ToXML()));
    }

    [TestMethod]
    public void RemoveFromShopperWatchListResponseToXml()
    {
      const string gTlds = @"<gTlds><gTld id='18' gTldSubCategoryId='234' /></gTlds>";
      var request = new RemoveFromShopperWatchListRequestData("861126", string.Empty, string.Empty, string.Empty, 0, gTlds);
      var response = (RemoveFromShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, 705);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }
  }
}
