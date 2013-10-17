
using System.Collections.Generic;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Providers.Interface.Products
{
  public interface IProductPackageItem
  {
    string Name { get; }
    int ProductId { get; set; }
    ICurrencyPrice CurrentPrice { get; }
    ICurrencyPrice ListPrice { get; }
    int Quantity { get; set; }
    double Duration { get; set; }
    IDictionary<string, string> BasketAttributes { get; set; }
    string CustomXml { get; }
    IList<IProductPackageItem> ChildItems { get; }
  }
}
