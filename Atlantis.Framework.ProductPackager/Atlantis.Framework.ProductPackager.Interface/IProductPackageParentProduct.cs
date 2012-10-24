using System.Collections.Generic;

namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductPackageParentProduct
  {
    int ProductId { get; }

    int Quantity { get; }

    double Duration { get; }

    string DiscountCode { get; }

    string DisplayOverride { get; }

    bool IsParent { get; }

    IList<IProductPackageChildProduct> ChildProducts { get; }
  }
}
