
namespace Atlantis.Framework.Providers.DomainProductPackage.Interface
{
  public interface IPersistanceStore
  {
    void Save(string key, string value);
    string Get(string key);
  }
}
