using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;

namespace Atlantis.Framework.Providers.DomainProductPackage
{
  public class DomainProductPackage : IDomainProductPackage
  {
    internal const string PACKAGE_NAME = "IDomainProductPackage.DomainProductPackage";
    private readonly IProviderContainer _container;
    private readonly Lazy<IShopperContext> _shopperContext;
    private readonly Lazy<ICurrencyProvider> _currencyProvider;

    public DomainProductPackage(IProviderContainer container)
    {
      _container = container;
      _shopperContext = new Lazy<IShopperContext>(_container.Resolve<IShopperContext>);
      _currencyProvider = new Lazy<ICurrencyProvider>(_container.Resolve<ICurrencyProvider>);
    }

    public IDomain Domain { get; set; }

    //public int RegistrationLength { get; private set; }

    //public void UpdateRegistrationLength(int registrationlength, int domainCount)
    //{
    //  var dotTypeInfo = DotTypeCache.DotTypeCache.GetDotTypeInfo(Domain.Tld);

    //  IDomainProductLookup lookup = new DomainProductLookup();
    //  lookup.DomainCount = domainCount;
    //  lookup.PriceTierId = TierId;
    //  lookup.Years = registrationlength;

    //  var productId = dotTypeInfo.GetProductId(lookup);

    //  DomainProductPackageItem.ProductId = productId;

    //  RegistrationLength = registrationlength;
    //}
    
    //public void SetRegAppToken()
    //{
      
    //}

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

    //public void SetPrivacy(string productPackageItemName)
    //{
    //  PrivacyRegistrationAttributes privacyAttributes = new PrivacyRegistrationAttributes();
    //  privacyAttributes.DomainByProxyAccount = _shopperContext.Value.ShopperId;

    //  var privacyPackage = new PrivacyPackageItem(Domain, privacyAttributes, _container);
    //  privacyPackage.Name = productPackageItemName;
    //  PackageItems.Add(privacyPackage);
    //}

    //public bool TryGetPrivacyPackageItem(string productPackageItemName, out IProductPackageItem privacyPackageItem)
    //{
    //  privacyPackageItem = PackageItems.FirstOrDefault(pi => pi.Name == productPackageItemName);
    //  var hasItem = privacyPackageItem != null && privacyPackageItem.Name == productPackageItemName;
    //  return hasItem;
    //}

    //public void SetDomainContacts()
    //{ 
      
    //}

    //public void SetCertifiedDomainOffer()
    //{
    //  var certifiedPackage = new CertifiedDomainPackageItem(Domain, DomainProductPackageItem.Duration);
    //  certifiedPackage.Duration = DomainProductPackageItem.Duration;
    //  certifiedPackage.Quantity = DomainProductPackageItem.Quantity;

    //  PackageItems.Add(certifiedPackage);
    //}

    //public bool TryGetCertifiedDomainOffer(string productPackageItemName, out IProductPackageItem productPackageItem)
    //{
    //  productPackageItem = PackageItems.FirstOrDefault(pi => pi.Name == productPackageItemName);
    //  var hasItem = productPackageItem != null && productPackageItem.Name == productPackageItemName;
    //  return hasItem;
    //}

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
        _currentPrice = _currencyProvider.Value.GetCurrentPrice(DomainProductPackageItem.ProductId);
        return _currentPrice;
      }
    }

    private ICurrencyPrice _listPrice;
    public ICurrencyPrice ListPrice
    {
      get
      {
        _listPrice = _currencyProvider.Value.GetListPrice(DomainProductPackageItem.ProductId);
        return _listPrice;
      }
    }

    // expose YearPrice and 
  }
}
