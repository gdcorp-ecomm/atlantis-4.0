using Atlantis.Framework.Providers.Basket.Interface;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.Basket
{
  public class BasketAddRequest : IBasketAddRequest
  {
    private readonly List<IBasketAddItem> _addItems;
    private readonly List<XElement> _additionalElements; 

    internal BasketAddRequest()
    {
      _addItems = new List<IBasketAddItem>(10);
      _additionalElements = new List<XElement>(1);
    }

    public void AddItem(IBasketAddItem item)
    {
      if (item != null)
      {
        _addItems.Add(item);
      }
    }

    public string ISC { get; set; }
    public string OrderDiscountCode { get; set; }
    public string FastballOrderDiscount { get; set; }
    public string TransactionCurrency { get; set; }
    public string YARD { get; set; }

    public void AddElement(XElement element)
    {
      if (element != null)
      {
        _additionalElements.Add(element);
      }
    }

    public IEnumerable<XElement> Elements
    {
      get { return _additionalElements; }
    }

    public IEnumerable<IBasketAddItem> Items
    {
      get { return _addItems; }
    }
  }
}
