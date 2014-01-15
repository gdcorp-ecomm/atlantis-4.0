using Atlantis.Framework.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.CDS.Interface
{
  public class UrlWhitelistResponseData : CDSResponseData
  {
    public ContentId VersionId { get; private set; }
    public ContentId DocumentId { get; private set; }
    private readonly Dictionary<string, IWhitelistResult> _whitelistDictionary;
    private const string STYLE = "style";

    private static readonly IWhitelistResult _notFoundWhitelistResult = new UrlWhitelistResult { Exists = false, UrlData = new Dictionary<string, string>() { { STYLE, DocumentStyles.Unknown } } };

    public static readonly IWhitelistResult NullWhitelistResult = new UrlWhitelistResult { Exists = true, UrlData = new Dictionary<string, string>() { { STYLE, DocumentStyles.Unknown } } };

    public UrlWhitelistResponseData(string responseData) : base(responseData)
    {
      ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);
      VersionId = contentVersion._id;
      DocumentId = contentVersion.DocumentId;

      var deserializedWhiteList = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(contentVersion.Content);
      
      if (deserializedWhiteList == null || deserializedWhiteList.Count == 0)
      {
        throw new Exception("UrlWhitelistResponseData empty white list.");
      }

      _whitelistDictionary = new Dictionary<string, IWhitelistResult>(deserializedWhiteList.Count, StringComparer.OrdinalIgnoreCase);

      foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in deserializedWhiteList)
      {
        _whitelistDictionary.Add(keyValuePair.Key, new UrlWhitelistResult { Exists = true, UrlData = keyValuePair.Value });
      }
    }

    public UrlWhitelistResponseData(RequestData requestData, Exception ex) : base(requestData, ex)
    {
    }

    public IWhitelistResult CheckWhitelist(string relativePath)
    {
      IWhitelistResult whitelistResult;
      
      if (_whitelistDictionary != null)
      {
        if (!_whitelistDictionary.TryGetValue(relativePath, out whitelistResult))
        {
          whitelistResult = _notFoundWhitelistResult;
        }
      }
      else
      {
        whitelistResult = NullWhitelistResult;
      }

      return whitelistResult;
    }
  }
}