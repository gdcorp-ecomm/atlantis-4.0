using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductPackageMapping : IProductPackageMapping
  {
    private readonly ProductGroupPackage _fbProductPackageMapping;

    private int? _productPackageId;
    public int ProductPackageId
    {
      get
      {
        if(!_productPackageId.HasValue)
        {
          _productPackageId = ParseHelper.ParseInt(_fbProductPackageMapping.pkgid, "ProductGroupPackage \"pkgid\" is not an integer. Value: {0}");
        }
        return _productPackageId.Value;
      }
    }

    private string _packageType;
    public string PackageType
    {
      get
      {
        if(_packageType == null)
        {
          _packageType = _fbProductPackageMapping.pkgType;
        }
        return _packageType;
      }
    }

    internal FbProductPackageMapping(ProductGroupPackage fbProductPackageMapping)
    {
      _fbProductPackageMapping = fbProductPackageMapping;
    }
  }
}
