using System;
using Atlantis.Framework.DotTypeEoi.Interface;
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
      var gTlds = @"<gTlds><gTld id='18' gTldSubCategoryId='234' /></gTlds>";
      var request = new AddToShopperWatchListRequestData("861126", string.Empty, string.Empty, string.Empty, 0, "2013-05-23 10:29:55", gTlds);
      var response = (AddToShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, 704);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ResponseMessage));
    }

    [TestMethod]
    public void AddToShopperWatchListBadRequest()
    {
      try
      {
        var request = new AddToShopperWatchListRequestData("861126", string.Empty, string.Empty, string.Empty, 0, string.Empty, string.Empty);
      }
      catch (Exception)
      {
        Assert.AreEqual(1, 1);        
      }
    }

    [TestMethod]
    public void AddToShopperWatchListRequestToXml()
    {
      const string gTlds = @"<gTlds><gTld id='18' gTldSubCategoryId='234' /></gTlds>";
      var request = new AddToShopperWatchListRequestData("861126", string.Empty, string.Empty, string.Empty, 0, "2013-05-23 10:29:55", gTlds);
      Assert.AreEqual(true, !string.IsNullOrEmpty(request.ToXML()));
    }

    [TestMethod]
    public void AddToShopperWatchListResponseToXml()
    {
      const string gTlds = @"<gTlds><gTld id='18' gTldSubCategoryId='234' /></gTlds>";
      var request = new AddToShopperWatchListRequestData("861126", string.Empty, string.Empty, string.Empty, 0, "2013-05-23 10:29:55", gTlds);
      var response = (AddToShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, 704);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

  }
}
