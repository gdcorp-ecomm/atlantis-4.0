using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  [TestClass]
  public class BasketDeleteRequestBuilderTests
  {
    [TestMethod]
    public void WillBuildBasketDeleteRequestDataReturnCorrectType()
    {
      var requestData = new BasketDeleteRequestBuilder().BuildRequestData(string.Empty,
        new Mock<IBasketDeleteRequest>().Object);

      Assert.IsNotNull(requestData);
      Assert.IsInstanceOfType(requestData, typeof(BasketDeleteRequestData));
    }

    [TestMethod]
    public void WillBuildBasketDeleteRequestDataSetShopperId()
    {
      const string shopperId = "1234";
      var requestData = new BasketDeleteRequestBuilder().BuildRequestData(shopperId,
        new Mock<IBasketDeleteRequest>().Object);

      Assert.IsTrue(string.Equals(shopperId, requestData.ShopperID));
    }

    [TestMethod]
    public void WillBuildBasketDeleteRequestDataSetItemsToDelete()
    {
      var item1 = new BasketDeleteItem (1, 2);
      var item2 = new BasketDeleteItem (3, 4);
      var deleteRequest = new BasketDeleteRequest();
      deleteRequest.AddItemToDelete(item1.RowId, item1.ItemId);
      deleteRequest.AddItemToDelete(item2.RowId, item2.ItemId);

      var requestData = new BasketDeleteRequestBuilder().BuildRequestData(string.Empty, deleteRequest);

      AssertBasketDeleteItemKey.AreEqual(requestData.ItemsToDelete, deleteRequest.ItemsToDelete);
    }
  }
}
