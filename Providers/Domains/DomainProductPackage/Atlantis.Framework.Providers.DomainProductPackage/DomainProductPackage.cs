using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.DomainProductPackage.PackageItems;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;

namespace Atlantis.Framework.Providers.DomainProductPackage
{
  public class DomainProductPackage : IDomainProductPackage
  {
    internal const string PACKAGE_NAME = "IDomainProductPackage.DomainProductPackage";
    internal const string APPLICATION_FEE = "IDomainProductPackage.DomainProductPackage";
    private readonly IProviderContainer _container;
    private readonly Lazy<IShopperContext> _shopperContext;
    private readonly Lazy<ICurrencyProvider> _currencyProvider;
    private readonly Lazy<IDotTypeProvider> _dotTypeProvider; 

    internal int ApplicationFeeProductId { get; set; }

    public DomainProductPackage(IProviderContainer container)
    {
      _container = container;
      _shopperContext = new Lazy<IShopperContext>(_container.Resolve<IShopperContext>);
      _currencyProvider = new Lazy<ICurrencyProvider>(_container.Resolve<ICurrencyProvider>);
      _dotTypeProvider = new Lazy<IDotTypeProvider>(() => container.Resolve<IDotTypeProvider>());
    }

    public IDomain Domain { get; set; }


    private IProductPackageItem _domainProductPackageItem;
    public IProductPackageItem DomainProductPackageItem
    {
      get
      {
        if (_domainProductPackageItem == null)
        {
          _domainProductPackageItem = PackageItems.FirstOrDefault(pi => pi.Name == PACKAGE_NAME);
        }

        return _domainProductPackageItem;
      }
    }
    
    IList<IProductPackageItem> _packageItems;
    public IList<IProductPackageItem> PackageItems
    {
      get
      {
        if (_packageItems == null)
        {
          _packageItems = new List<IProductPackageItem>(8);
        }

        return _packageItems;
      }
    }

    //public bool AutoRenew { get; set; }

    //public string User { get; set; }

    public int? TierId { get; set; }

    private ICurrencyPrice _currentPrice;
    public ICurrencyPrice CurrentPrice
    {
      get
      {
        var totalAmount = 0;
        _currentPrice = _currencyProvider.Value.GetCurrentPrice(DomainProductPackageItem.ProductId);

        foreach (var productPackageItem in PackageItems)
        {
          totalAmount += productPackageItem.CurrentPrice.Price;
        }

        if (totalAmount > 0)
        {
          var currencyInfo = _currentPrice.CurrencyInfo;
          _currentPrice = new CurrencyPrice(totalAmount, currencyInfo, CurrencyPriceType.Transactional);

        }

        return _currentPrice;
      }
    }

    private ICurrencyPrice _listPrice;
    public ICurrencyPrice ListPrice
    {
      get
      {
        var totalAmount = 0;
        _listPrice = _currencyProvider.Value.GetListPrice(DomainProductPackageItem.ProductId);

        foreach (var productPackageItem in PackageItems)
        {
          totalAmount += productPackageItem.ListPrice.Price;
        }

        if (totalAmount > 0)
        {
          _listPrice = new CurrencyPrice(totalAmount, _listPrice.CurrencyInfo, CurrencyPriceType.Transactional);

        }
        return _listPrice;
      }
    }


    public bool TryGetApplicationFee(out ICurrencyPrice applicationFee)
    {
      applicationFee = null;

      foreach (IProductPackageItem productPackageItem in PackageItems)
      {
        if (productPackageItem.Name == APPLICATION_FEE)
        {
          applicationFee = productPackageItem.CurrentPrice;
        }
      }

      return applicationFee != null;
    }
  }
}
