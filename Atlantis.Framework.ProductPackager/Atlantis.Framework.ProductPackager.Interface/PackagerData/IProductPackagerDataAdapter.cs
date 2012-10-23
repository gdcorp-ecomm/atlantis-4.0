using System.Collections.Generic;

namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductPackagerDataAdapter
  {
    IList<IProductGroup> GetProductGroupData<T>(T productGroupDataSource);

    IList<IProductPackageData> GetProductPackageData<T>(T productPackageDataSource);
  }
}
