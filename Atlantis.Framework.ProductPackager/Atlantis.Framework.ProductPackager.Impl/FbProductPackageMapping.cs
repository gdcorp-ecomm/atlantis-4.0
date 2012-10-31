using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductPackageMapping : IProductPackageMapping
  {
    private readonly ProductGroupPackage _fbProductPackageMapping;

    private string _productPackageId;
    public string ProductPackageId
    {
      get
      {
        if(_productPackageId == null)
        {
          _productPackageId = _fbProductPackageMapping.pkgid;
        }
        return _productPackageId;
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
