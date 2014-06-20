using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Atlantis.Framework.Basket.Interface;
using Atlantis.Framework.Providers.Basket.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  [ExcludeFromCodeCoverage]
  internal static class AssertBasketDeleteItemKey
  {
    internal static void AreEqual(BasketDeleteItemKey basketDeleteItemKey,
      IBasketDeleteItem basketDeleteItem)
    {
      if (basketDeleteItemKey == null)
      {
        Assert.IsNull(basketDeleteItem);
      }
      else
      {
        Assert.IsNotNull(basketDeleteItem);
        Assert.AreEqual(basketDeleteItemKey.RowId, basketDeleteItem.RowId);
        Assert.AreEqual(basketDeleteItemKey.ItemId, basketDeleteItem.ItemId);
      }
    }

    internal static void AreEqual(IEnumerable<BasketDeleteItemKey> basketDeleteItemKeys,
      IEnumerable<IBasketDeleteItem> basketDeleteItems)
    {
      if (basketDeleteItemKeys == null)
      {
        Assert.IsNull(basketDeleteItems);
      }
      else
      {
        Assert.IsNotNull(basketDeleteItems);

        var basketDeleteItemKeyList = basketDeleteItemKeys.OrderBy(d => d.RowId).ToList();
        var basketDeleteItemList = basketDeleteItems.OrderBy(d => d.RowId).ToList();

        Assert.AreEqual(basketDeleteItemKeyList.Count, basketDeleteItemList.Count);
        for (var i = 0; i < basketDeleteItemKeyList.Count; i++)
        {
          AreEqual(basketDeleteItemKeyList[i], basketDeleteItemList[i]);
        }
      }
    }
  }
}
