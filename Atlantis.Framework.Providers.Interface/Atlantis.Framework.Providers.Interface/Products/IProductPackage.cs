
using System.Collections.Generic;
using Atlantis.Framework.Providers.Interface.Currency;

namespace Atlantis.Framework.Providers.Interface.Products
{
  public interface IProductPackage
  {
    ICurrencyPrice CurrentPrice { get; }
    ICurrencyPrice ListPrice { get; }

    //IList<IProductPackageItem> PackageItems { get; }

    //bool AutoRenew { get; }
  }
}
