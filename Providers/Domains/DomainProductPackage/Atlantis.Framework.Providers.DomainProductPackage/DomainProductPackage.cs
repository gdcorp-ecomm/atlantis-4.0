using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainProductPackage.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.DomainProductPackage.PackageItems;

namespace Atlantis.Framework.Providers.DomainProductPackage
{
  public class DomainProductPackage : IDomainProductPackage
  {
    public const string PACKAGE_NAME = "DomainProductPackage.DomainProductPackageItem";
    public const string APPLICATION_FEE_NAME = "DomainProductPackage.ApplicationFeePackageItem";
    public const string PRIVACY_NAME = "DomainProductPackage.PrivacyPackageItem";
    public const string TRUSTEE_NAME = "DomainProductPackage.TrusteePackageItem";

    private readonly Lazy<ICurrencyProvider> _currencyProvider;
    private readonly Lazy<IDotTypeProvider> _dotTypeProvider;

    internal int ApplicationFeeProductId { get; set; }

    public DomainProductPackage(IProviderContainer container)
    {
      _currencyProvider = new Lazy<ICurrencyProvider>(container.Resolve<ICurrencyProvider>);
      _dotTypeProvider = new Lazy<IDotTypeProvider>(container.Resolve<IDotTypeProvider>);
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
          _currentPrice = _currencyProvider.Value.NewCurrencyPrice(totalAmount, currencyInfo, CurrencyPriceType.Transactional);

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
          _listPrice =  _currencyProvider.Value.NewCurrencyPrice(totalAmount, _listPrice.CurrencyInfo, CurrencyPriceType.Transactional);

        }
        return _listPrice;
      }
    }

    public bool TryGetApplicationFee(out ICurrencyPrice applicationFeePrice)
    {
      applicationFeePrice = null;

      IProductPackageItem packageItem;
      if (TryGetApplicationFeePackage(out packageItem))
      {
        applicationFeePrice = packageItem.CurrentPrice;
      }

      return applicationFeePrice != null;
    }

    public bool TryGetApplicationFeePackage(out IProductPackageItem applicationFeePackageItem)
    {
      applicationFeePackageItem = PackageItems.FirstOrDefault(package => package.Name == APPLICATION_FEE_NAME);

      return applicationFeePackageItem != null;
    }

    public bool TryGetTrusteePackage(out IProductPackageItem trusteeRegistrationProductPackage)
    {
      trusteeRegistrationProductPackage = PackageItems.FirstOrDefault(package => package.Name == TRUSTEE_NAME);

      return trusteeRegistrationProductPackage != null;
    }

    public int? TierId { get; set; }

  }
}
