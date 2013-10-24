
namespace Atlantis.Framework.Providers.DomainProductPackageStateProvider
{
  public interface IPersistanceStore
  {
    void Save(string key, string value);
    string Get(string key);
  }
}
