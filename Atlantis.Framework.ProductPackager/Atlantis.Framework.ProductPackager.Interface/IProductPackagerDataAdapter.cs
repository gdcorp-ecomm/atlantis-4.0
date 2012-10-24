using System.Collections.Generic;

namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductPackagerDataAdapter
  {
    IDictionary<string, IProductGroup> GetProductGroupData<T>(T productGroupDataSource);

    IDictionary<string, IProductPackageData> GetProductPackageData<T>(T productPackageDataSource);
  }
}
