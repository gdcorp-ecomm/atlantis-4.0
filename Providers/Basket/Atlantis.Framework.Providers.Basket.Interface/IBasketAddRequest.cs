using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.Basket.Interface
{
  /// <summary>
  /// Request collector used to build up all items to be added to the basket in a single call to the basket service
  /// </summary>
  public interface IBasketAddRequest
  {
    /// <summary>
    /// Adds an IBasketItem to the request
    /// </summary>
    /// <param name="item">IBasketItem to add</param>
    void AddItem(IBasketAddItem item);

    /// <summary>
    /// ISC to apply to the basket
    /// </summary>
    string ISC { get; set; }

    /// <summary>
    /// Order Discount Code to apply to the basket
    /// </summary>
    string OrderDiscountCode { get; set; }

    /// <summary>
    /// Fastball Order Discount to apply to the basket
    /// </summary>
    string FastballOrderDiscount { get; set; }

    /// <summary>
    /// Transactional currency to apply to the basket
    /// </summary>
    string TransactionCurrency { get; set; }

    /// <summary>
    /// YARD value
    /// </summary>
    string YARD { get; set; }
  
    /// <summary>
    /// Adds a request level element at the same level as the item elements.
    /// </summary>
    /// <param name="element"></param>
    void AddElement(XElement element);

    /// <summary>
    /// Returns an IEnumerable set of Elements
    /// </summary>
    IEnumerable<XElement> Elements { get; }

    /// <summary>
    /// Returns an IEnumerable of IBasketAddItems
    /// </summary>
    IEnumerable<IBasketAddItem> Items { get; } 
  }
}
