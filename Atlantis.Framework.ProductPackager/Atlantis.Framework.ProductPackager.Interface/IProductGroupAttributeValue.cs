using System.Collections.Generic;

namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductGroupAttributeValue
  {
    int Id { get; }

    string Name { get; }

    string Value { get; }

    bool IsDefault { get; }

    IList<IProductData> ProductDataList { get; }
  }
}
