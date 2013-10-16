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
    IProductPackageItem DomainProductPackageItem { get; }


    // bool TryGetApplicationFee
    
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