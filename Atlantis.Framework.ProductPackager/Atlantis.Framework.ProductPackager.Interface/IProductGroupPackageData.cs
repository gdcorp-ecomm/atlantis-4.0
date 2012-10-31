using System.Collections.Generic;

namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductGroupPackageData
  {
    int ProductId { get; }

    int Quantity { get; }
    
    double? Duration { get; }

    IList<IProductPackageMapping> ProductPackageMappings { get; }
  }
}
