using System.Collections.Specialized;

namespace Atlantis.Framework.Providers.MobileRedirect.Interface
{
  public interface IMobileRedirectProvider
  {
    bool IsRedirectRequired();

    string GetRedirectUrl(string redirectKey, NameValueCollection additionalQueryParameters);
  }
}
