using System;
using System.Collections.Generic;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  public class FbProductPackagerDataAdapter : IProductPackagerDataAdapter
  {
    public IList<IProductGroup> GetProductGroupData<T>(T productGroupDataSource)
    {
      ProductGroupData fbProductGroupData = productGroupDataSource as ProductGroupData;
      if(fbProductGroupData == null)
      {
        throw new Exception("Type of productGroupDataSource is expect to be ProductGroupData from the ProductPackage soap service.");   
      }

      IList<IProductGroup> productGroupList = new List<IProductGroup>(fbProductGroupData.ProdGroups.Length);

      foreach (ProductGroup fbProductGroup in fbProductGroupData.ProdGroups)
      {
        productGroupList.Add(new FbProductGroup(fbProductGroup));
      }

      return productGroupList;
    }

    public IList<IProductPackageData> GetProductPackageData<T>(T productPackageDataSource)
    {
      throw new NotImplementedException();
    }
  }
}
