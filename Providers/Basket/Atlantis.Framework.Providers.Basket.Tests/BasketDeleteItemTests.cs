using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  [TestClass]
  public class BasketDeleteItemTests
  {
    [TestMethod]
    public void WillSetAndGetRowId()
    {
      const int rowId = 47;
      var item = new BasketDeleteItem (rowId, 1);

      Assert.AreEqual(rowId, item.RowId);
    }

    [TestMethod]
    public void WillSetAndGetItemId()
    {
      const int itemId = 32;
      var item = new BasketDeleteItem (1, itemId);

      Assert.AreEqual(itemId, item.ItemId);
    }
  }
}
