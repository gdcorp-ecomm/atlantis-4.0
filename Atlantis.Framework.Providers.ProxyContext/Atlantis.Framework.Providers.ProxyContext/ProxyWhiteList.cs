using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.ProxyContext
{
  internal class ProxyWhiteList
  {
    const string _PROXYCACHEREQUEST = "<ProxyIPWhiteListSelect><param name=\"gdshop_proxyTypeID\" value=\"{0}\" /></ProxyIPWhiteListSelect>";
    readonly HashSet<string> _whiteList = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

    private ProxyWhiteList(string proxyTypeId)
    {
      try
      {
        string cacheDataRequest = string.Format(_PROXYCACHEREQUEST, proxyTypeId);
        string xmlData;

        using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          xmlData = comCache.GetCacheData(cacheDataRequest);
        }

        if (!string.IsNullOrEmpty(xmlData))
        {
          XElement xmlDoc = XElement.Parse(xmlData);
          var items = xmlDoc.Descendants("item");
          foreach (var item in items)
          {
            XAttribute ipAddressAttribute = item.Attribute("IPAddress");
            if (ipAddressAttribute != null)
            {
              _whiteList.Add(ipAddressAttribute.Value);
            }
          }
        }
        else
        {
          AtlantisException aex = new AtlantisException("ProxyWhiteList.Constructor", "0", "No data returned from CacheData call.", proxyTypeId, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message + Environment.NewLine + ex.StackTrace;
        AtlantisException aex = new AtlantisException("ProxyWhiteList.Constructor", "0", message, proxyTypeId, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }
           
    }

    public bool IsValidProxyIP(string ipAddress)
    {
      return _whiteList.Contains(ipAddress);
    }

    internal static ProxyWhiteList LoadProxyWhiteList(string proxyTypeId)
    {
      return new ProxyWhiteList(proxyTypeId);
    }
  }
}
