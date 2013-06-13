using Atlantis.Framework.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace Atlantis.Framework.CDS.Interface
{
  public class UrlWhitelistResponseData : CDSResponseData
  {
    private Dictionary<string, IWhitelistResult> _whitelist = null;
    public static IUrlData NullUrlData = new UrlData() { Style = string.Empty };
    public static IWhitelistResult NullWhielistResult = new UrlWhitelistResult() { Exists = false, UrlData = NullUrlData };

    public UrlWhitelistResponseData(string responseData)
      : base(responseData)
    {
      ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);
      //duplicates in the key are ignored in this dictionary deserialization
      Dictionary<string, UrlData> _temp = JsonConvert.DeserializeObject<Dictionary<string, UrlData>>(contentVersion.Content);
      _whitelist = new Dictionary<string, IWhitelistResult>(StringComparer.OrdinalIgnoreCase);
      foreach (var t in _temp)
      {
        _whitelist.Add(t.Key, new UrlWhitelistResult() { Exists = true, UrlData = t.Value });
      }
    }

    public UrlWhitelistResponseData(RequestData requestData, Exception exception)
      : base(requestData, exception)
    {
    }

    public IWhitelistResult CheckWhitelist(string relativePath)
    {
      IWhitelistResult temp = null;
      if (_whitelist != null)
      {
        //case-insensitive comparison
        _whitelist.TryGetValue(relativePath, out temp);
      }

      return temp ?? NullWhielistResult;
    }
  }
}
