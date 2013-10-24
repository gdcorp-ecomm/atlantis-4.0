
using System.Web;

namespace Atlantis.Framework.Providers.DomainProductPackageStateProvider
{
  public class PersistanceStore : IPersistanceStore
  {
    public virtual void Save(string key, string packagesString)
    {
      HttpContext.Current.Session[key] = packagesString;
    }

    public virtual string Get(string key)
    {
      var result = HttpContext.Current.Session[key] as string;

      return result;
    }
  }
}