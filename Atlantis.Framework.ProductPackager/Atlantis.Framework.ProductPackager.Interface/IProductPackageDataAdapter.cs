using System.Collections.Generic;

namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductPackageDataAdapter
  {
    IList<IProductGroup> GetProductGroupData<T>(T productGroupDataSource);

    IList<IProductPackageData> GetProductPackageData<T>(T productPackageDataSource);
  }
}
