using System.Collections.Generic;

namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductPackageDataSourceAdapter
  {
    IDictionary<string, object> TransformData(IProductGroup productGroup);
  }
}
