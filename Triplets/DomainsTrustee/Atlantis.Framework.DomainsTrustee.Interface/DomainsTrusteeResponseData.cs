using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Atlantis.Framework.DomainsTrustee.Interface
{
  public class DomainsTrusteeResponseData : IResponseData
  {
    readonly AtlantisException _exception;
    private readonly string _rawJsonResponse;

    private DomainsTrusteeResponseData(string rawJson)
    {
      _rawJsonResponse = rawJson;
      ParseRawJsonResponse();
    }

    private DomainsTrusteeResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    private void ParseRawJsonResponse()
    {
      if (!string.IsNullOrEmpty(_rawJsonResponse))
      {
        try
        {
          var reader = new JsonTextReader(new StringReader(_rawJsonResponse));
          var jsonData = new JsonSerializer().Deserialize(reader) as IEnumerable<JToken>;
          if (jsonData != null)
          {
            _domainsTrustees = GetResponseDomainTrustees(jsonData);
          }
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException("DomainsTrusteeResponseData.ParseRawJsonResponse", "0", ex.ToString(), _rawJsonResponse, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
    }

    private Dictionary<string, DomainsTrusteeResponse> GetResponseDomainTrustees(IEnumerable<JToken> json)
    {
      _domainsTrustees = new Dictionary<string, DomainsTrusteeResponse>(StringComparer.OrdinalIgnoreCase);

      var enumerable = json as JToken[] ?? json.ToArray();
      if (json != null && enumerable.Any())
      {
        foreach (var token in enumerable)
        {
          var item = new DomainsTrusteeResponse
          {
            NameWithoutExtension = token["Name"].ToString(),
            Tld = token["Tld"].ToString(),
            TrusteeEnabled = token["TrusteeEnabled"].ToString() == "true",
            LocalPresence = token["LocalPresence"].ToString() == "true",
            TuiFormType = token["TUIFormType"].ToString(),
            VendorId = token["VendorId"].ToString(),
            Error = token["Error"].ToString()
          };
          _domainsTrustees[item.NameWithoutExtension + "." + item.Tld] = item;
        }
      }

      return _domainsTrustees;
    }

    private Dictionary<string, DomainsTrusteeResponse> _domainsTrustees;

    public bool TryGetDomainTrustee(string nameWithoutExtension, string tld, out DomainsTrusteeResponse domainTrustee)
    {
      var key = nameWithoutExtension + "." + tld;
      return _domainsTrustees.TryGetValue(key, out domainTrustee);
    }

    public static IResponseData ParseRawResponse(string rawJson)
    {
      return new DomainsTrusteeResponseData(rawJson);
    }

    public static IResponseData FromAtlantisException(AtlantisException exception)
    {
      return new DomainsTrusteeResponseData(exception);
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public string ToJson()
    {
      return _rawJsonResponse ?? string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
