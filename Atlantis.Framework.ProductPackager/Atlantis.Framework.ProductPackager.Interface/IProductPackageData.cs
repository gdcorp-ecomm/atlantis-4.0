
namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductPackageData
  {
    string Id { get; }

    string PackageType { get; }

    IProductPackageParentProduct ParentProduct { get; }
  }
}
