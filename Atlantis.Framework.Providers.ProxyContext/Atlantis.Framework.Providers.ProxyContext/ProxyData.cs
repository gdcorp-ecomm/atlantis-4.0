using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.ProxyContext
{
  public class ProxyData : IProxyData
  {
    private bool _isContextualHost = false;
    private string _originalHost = string.Empty;
    private string _originalIP = string.Empty;
    private Dictionary<string, string> _extendedData;
    private ProxyTypes _proxyType = ProxyTypes.None;

    internal static ProxyData FromValidData(ProxyTypes proxyType, string originalIP, string originalHost, bool isContextualHost, IDictionary<string, string> extendedData = null)
    {
      return new ProxyData(proxyType, originalIP, originalHost, isContextualHost, extendedData);
    }

    internal static ProxyData InvalidData(ProxyTypes proxyType)
    {
      return new ProxyData(proxyType, string.Empty, string.Empty, false, null);
    }

    private ProxyData(ProxyTypes proxyType, string originalIP, string originalHost, bool isContextualHost, IDictionary<string, string> extendedData)
    {
      _proxyType = proxyType;
      _originalIP = originalIP;
      _originalHost = originalHost;
      _isContextualHost = isContextualHost;

      _extendedData = null;
      if (extendedData != null)
      {
        _extendedData = new Dictionary<string, string>(extendedData, StringComparer.OrdinalIgnoreCase);
      }
    }

    public bool IsContextualHost
    {
      get { return _isContextualHost; }
    }

    public string OriginalHost
    {
      get { return _originalHost; }
    }

    public string OriginalIP
    {
      get { return _originalIP; }
    }

    public ProxyTypes ProxyType
    {
      get { return _proxyType; }
    }

    public bool TryGetExtendedData(string key, out string value)
    {
      value = null;

      if (_extendedData != null)
      {
        return _extendedData.TryGetValue(key, out value);
      }
      else
      {
        return false;
      }
    }
  }
}
