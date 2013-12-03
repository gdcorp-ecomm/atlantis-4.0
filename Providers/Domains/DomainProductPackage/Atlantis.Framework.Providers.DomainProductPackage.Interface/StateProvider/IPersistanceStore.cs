
namespace Atlantis.Framework.Providers.DomainProductPackage.Interface.StateProvider
{
  public interface IPersistanceStore
  {
    void Save(string key, string value);
    string Get(string key);
  }
}
