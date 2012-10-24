using System.Collections.Generic;

namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductGroupAttribute
  {
    string Name { get; }

    IList<IProductGroupAttributeValue> ProductGroupAttributeValues { get; }
  }
}
