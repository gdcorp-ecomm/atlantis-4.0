using Atlantis.Framework.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace Atlantis.Framework.CDS.Interface
{
  public class UrlWhitelistResponseData : CDSResponseData
  {
    private class UrlWhiteList
    {
      [JsonProperty("urls")]
      public Dictionary<string, string> List { get; set; }
    }

    private UrlWhiteList _urlWhiteList;

    public UrlWhitelistResponseData(string responseData)
      : base(responseData)
    {
      ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);
      _urlWhiteList = JsonConvert.DeserializeObject<UrlWhiteList>(contentVersion.Content);
    }

    public UrlWhitelistResponseData(RequestData requestData, Exception exception)
      : base(requestData, exception)
    {
    }

    /// <summary>
    /// Does a case-sensitive search for the given string with-in the UrlWhiteList and returns true if found.
    /// </summary>
    /// <param name="relativePath">Example: "hosting/web-hosting.aspx"</param>
    /// <returns>true/false</returns>
    public bool TryGetValue(string relativePath, out string style)
    {
      style = null;
      bool contains = false;
      if (_urlWhiteList != null && _urlWhiteList.List != null)
      {
        contains = _urlWhiteList.List.TryGetValue(relativePath, out style);
      }
      return contains;
    }
  }
}
