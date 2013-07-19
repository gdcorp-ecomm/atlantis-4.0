using Atlantis.Framework.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.CDS.Interface
{
  public class UrlWhitelistResponseData : CDSResponseData
  {
    private readonly Dictionary<string, IWhitelistResult> _whitelist = new Dictionary<string, IWhitelistResult>(StringComparer.OrdinalIgnoreCase);
    
    private static readonly IUrlData _nullUrlData = new UrlData { Style = string.Empty };
    
    public static IWhitelistResult DefaultWhitelistResult = new UrlWhitelistResult { Exists = true, UrlData = _nullUrlData };

    public UrlWhitelistResponseData(string responseData) : base(responseData)
    {
      ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);

      Dictionary<string, UrlData> deserializedWhiteList = JsonConvert.DeserializeObject<Dictionary<string, UrlData>>(contentVersion.Content);
      
      foreach (KeyValuePair<string, UrlData> keyValuePair in deserializedWhiteList)
      {
        _whitelist.Add(keyValuePair.Key, new UrlWhitelistResult { Exists = true, UrlData = keyValuePair.Value });
      }
    }

    public UrlWhitelistResponseData(RequestData requestData, Exception exception) : base(requestData, exception)
    {
    }

    public IWhitelistResult CheckWhitelist(string relativePath)
    {
      IWhitelistResult whitelistResult = null;
      if (_whitelist != null)
      {
        _whitelist.TryGetValue(relativePath, out whitelistResult);
      }

      return whitelistResult ?? DefaultWhitelistResult;
    }
  }
}
