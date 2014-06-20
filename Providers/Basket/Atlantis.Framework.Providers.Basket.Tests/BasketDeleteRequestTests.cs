using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  [TestClass]
  public class BasketDeleteRequestTests
  {
    [TestMethod]
    public void WillBasketDeleteRequestConstructorInitializeItemsToDelete()
    {
      var request = new BasketDeleteRequest();
      Assert.IsNotNull(request.ItemsToDelete);
    }

    [TestMethod]
    public void WillBasketDeleteRequestAddItem()
    {
      var item = new BasketDeleteItem (1, 2);
      var request = new BasketDeleteRequest();
      request.AddItemToDelete(item.RowId, item.ItemId);
      AssertIBasketDeleteItem.AreEqual(new[] { item }, request.ItemsToDelete);
    }
  }
}
