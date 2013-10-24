using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;

namespace Atlantis.Framework.Providers.DomainProductPackage.PackageItems
{
  public class ProductPackageItem : IProductPackageItem
  {
    private readonly Lazy<ICurrencyProvider> _currencyProvider;

    public static ProductPackageItem Create(string name, int productId, int quantity, double duration, IProviderContainer container)
    {
      var productPackageItem = new ProductPackageItem(name, container) {ProductId = productId, Duration = duration, Quantity = quantity};

      return productPackageItem;
    }

    private ProductPackageItem(string name, IProviderContainer container)
    {
      _name = name;
      _currencyProvider = new Lazy<ICurrencyProvider>(container.Resolve<ICurrencyProvider>);
    }
    
    private readonly string _name;
    public string Name
    {
      get { return _name; }
    }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public double Duration { get; set; }

    private IDictionary<string, string> _basketAttributes = new Dictionary<string, string>(0);
    public IDictionary<string, string> BasketAttributes
    {
      get
      {
        return _basketAttributes;
      }
      set { _basketAttributes = value; }
    }

    public string CustomXml
    {
      get { return string.Empty; }
    }

    private IList<IProductPackageItem> _childItems;
    public IList<IProductPackageItem> ChildItems
    {
      get
      {
        if (_childItems == null)
        {
          _childItems = new List<IProductPackageItem>(0);
        }

        return _childItems;
      }
    }

    private ICurrencyPrice _currentPrice;
    public ICurrencyPrice  CurrentPrice
    {
      get {
        _currentPrice = _currencyProvider.Value.GetCurrentPrice(ProductId);
        return _currentPrice;
      }
    }

    private ICurrencyPrice _listPrice;
    public ICurrencyPrice ListPrice
    {
      get
      {
        _listPrice = _currencyProvider.Value.GetListPrice(ProductId);
        return _listPrice;
      }
    }
  }
}
