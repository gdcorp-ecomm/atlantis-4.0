using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductPackageData : IProductPackageData
  {
    public string Id { get; private set; }

    public string PackageType { get; private set; }

    public IProductPackageParentProduct ParentProduct { get; private set; }

    private FbProductPackageData(string id, string packageType, IProductPackageParentProduct parentProduct)
    {
      Id = id;
      PackageType = packageType;
      ParentProduct = parentProduct;
    }

    internal FbProductPackageData(CartProdPackage prodPackage) : this(prodPackage.pkgid, ProductPackageTypes.Cart, new FbProductPackageParentProduct(prodPackage.ParentProd))
    {
    }

    internal FbProductPackageData(AddOnProdPackage prodPackage) : this(prodPackage.pkgid, ProductPackageTypes.AddOn, new FbProductPackageParentProduct(prodPackage.ParentProd))
    {
    }

    internal FbProductPackageData(UpsellProdPackage prodPackage) : this(prodPackage.pkgid, ProductPackageTypes.UpSell, new FbProductPackageParentProduct(prodPackage.ParentProd))
    {
    }
  }
}
