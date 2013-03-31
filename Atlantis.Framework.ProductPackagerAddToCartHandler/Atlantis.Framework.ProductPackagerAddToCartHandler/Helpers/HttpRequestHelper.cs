using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.ProductPackagerAddToCartHandler
{
  internal static class HttpRequestHelper
  {
    internal static string ClientIp
    {
      get
      {
        string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (!string.IsNullOrEmpty(clientIp))
        {
          string[] ipRange = clientIp.Split(',');
          if (ipRange.Length > 0)
          {
            clientIp = ipRange[0].Trim();
          }
        }

        if (string.IsNullOrEmpty(clientIp))
        {
          clientIp = HttpContext.Current.Request.UserHostAddress;
        }

        IProxyContext proxyContext;
        if (HttpProviderContainer.Instance.TryResolve(out proxyContext))
        {
          clientIp = proxyContext.OriginIP;
        }

        return clientIp;
      }
    }
  }
}
