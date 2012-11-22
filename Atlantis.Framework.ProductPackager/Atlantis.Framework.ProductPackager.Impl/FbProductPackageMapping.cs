using System.Text.RegularExpressions;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;

namespace Atlantis.Framework.ProductPackager.Impl
{
  internal class FbProductPackageMapping : IProductPackageMapping
  {
    private static readonly Regex _removeNonAlphaCharacters = new Regex(@"[^a-zA-Z]", RegexOptions.Compiled);

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
          _packageType = _removeNonAlphaCharacters.Replace(_fbProductPackageMapping.pkgType, string.Empty).ToLowerInvariant(); // We need to make sure "Add-ons" comes back as "addons" to match the product package xml
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
