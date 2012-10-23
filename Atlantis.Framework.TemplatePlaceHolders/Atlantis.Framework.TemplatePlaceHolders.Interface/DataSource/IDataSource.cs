
namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  public interface IDataSource
  {
    string ProviderAssembly { get; }

    string ProviderType { get; }

    string GetDataSource(string dataSourceKey);
  }
}
