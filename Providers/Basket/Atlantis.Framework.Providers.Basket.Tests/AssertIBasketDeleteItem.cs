using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Atlantis.Framework.Providers.Basket.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Basket.Tests
{
  [ExcludeFromCodeCoverage]
  internal static class AssertIBasketDeleteItem
  {
    internal static void AreEqual(IBasketDeleteItem expectedBasketDeleteItem,
      IBasketDeleteItem actualBasketDeleteItem)
    {
      if (expectedBasketDeleteItem == null)
      {
        Assert.IsNull(actualBasketDeleteItem);
      }
      else
      {
        Assert.IsNotNull(actualBasketDeleteItem);
        Assert.AreEqual(expectedBasketDeleteItem.RowId, actualBasketDeleteItem.RowId);
        Assert.AreEqual(expectedBasketDeleteItem.ItemId, actualBasketDeleteItem.ItemId);
      }
    }

    internal static void AreEqual(IEnumerable<IBasketDeleteItem> expectedBasketDeleteItems,
      IEnumerable<IBasketDeleteItem> actualBasketDeleteItems)
    {
      if (expectedBasketDeleteItems == null)
      {
        Assert.IsNull(actualBasketDeleteItems);
      }
      else
      {
        Assert.IsNotNull(actualBasketDeleteItems);

        var expectedList = expectedBasketDeleteItems.OrderBy(d => d.RowId).ToList();
        var actualList = actualBasketDeleteItems.OrderBy(d => d.RowId).ToList();

        Assert.AreEqual(expectedList.Count, actualList.Count);
        for (var i = 0; i < expectedList.Count; i++)
        {
          AreEqual(expectedList[i], actualList[i]);
        }
      }
    }
  }
}
