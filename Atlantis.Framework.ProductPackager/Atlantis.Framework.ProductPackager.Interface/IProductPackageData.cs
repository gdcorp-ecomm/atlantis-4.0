
namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductPackageData
  {
    int Id { get; }

    string PackageType { get; }

    IProductPackageParentProduct ParentProduct { get; }
  }
}
