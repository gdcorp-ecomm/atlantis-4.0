using System.Collections.Generic;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;

namespace Atlantis.Framework.Domains.Interface
{
  /// <summary>
  /// Prepare Domain items for cart
  /// </summary>
  public interface IDomainProductPackage : IProductPackage
  {
    int? TierId { get; set; }

    IProductPackageItem DomainProductPackageItem { get; }

    IList<IProductPackageItem> PackageItems { get; }

    bool TryGetApplicationFee(out ICurrencyPrice applicationFee);

    bool TryGetApplicationFeePackage(out IProductPackageItem productPackageItem);

    //void SetPrivacy(string privacyPackageItemName);

    /// <summary>
    /// Set Domain Contact information  
    /// </summary>
    //void SetDomainContacts();

    /// <summary>
    /// Set Certified Domain product ids, registration lengths and tracking code.
    /// </summary>
    //void SetCertifiedDomainOffer();

    //void SetRegAppToken();
  }
}