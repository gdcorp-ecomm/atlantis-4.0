using System;
using System.Collections.Generic;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  public class FbProductPackagerDataAdapter : IProductPackagerDataAdapter
  {
    public IDictionary<string, IProductGroup> GetProductGroupData<T>(T productGroupDataSource)
    {
      ProductGroupData fbProductGroupData = productGroupDataSource as ProductGroupData;
      if(fbProductGroupData == null)
      {
        throw new Exception("Type of productGroupDataSource is expected to be ProductGroupData from the ProductPackage soap service.");   
      }

      IDictionary<string, IProductGroup> productGroupList = new Dictionary<string, IProductGroup>(fbProductGroupData.ProdGroups.Length);

      foreach (ProductGroup fbProductGroup in fbProductGroupData.ProdGroups)
      {
        FbProductGroup productGroup = new FbProductGroup(fbProductGroup);
        productGroupList[productGroup.Id] = productGroup;
      }

      return productGroupList;
    }

    public IDictionary<string, IProductPackageData> GetProductPackageData<T>(T productPackageDataSource)
    {
      ProdPackages fbProductPackageData = productPackageDataSource as ProdPackages;
      if(fbProductPackageData == null)
      {
        throw new Exception("Type of productPackageDataSource is expected to be ProdPackages from the ProductPackage soap service.");   
      }

      IDictionary<string, IProductPackageData> productPackageDictionary = new Dictionary<string, IProductPackageData>(fbProductPackageData.CartPackages.Length + 
                                                                                                                      fbProductPackageData.AddonPackages.Length +
                                                                                                                      fbProductPackageData.UpsellPackages.Length);

      foreach (CartProdPackage productPackage in fbProductPackageData.CartPackages)
      {
        productPackageDictionary[productPackage.pkgid] = new FbProductPackageData(productPackage);
      }

      foreach (AddOnProdPackage productPackage in fbProductPackageData.AddonPackages)
      {
        productPackageDictionary[productPackage.pkgid] = new FbProductPackageData(productPackage);
      }

      foreach (UpsellProdPackage productPackage in fbProductPackageData.UpsellPackages)
      {
        productPackageDictionary[productPackage.pkgid] = new FbProductPackageData(productPackage);
      }

      return productPackageDictionary;
    }
  }
}
