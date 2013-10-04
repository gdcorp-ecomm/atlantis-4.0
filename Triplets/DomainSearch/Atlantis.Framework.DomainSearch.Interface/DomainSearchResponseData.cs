using System.Collections.Generic;
using System.IO;
using System.Linq;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.Interface;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.DomainSearch.Interface
{
  public class DomainSearchResponseData : IResponseData
  {
    readonly AtlantisException _exception;
    private readonly string _rawJsonResponse;

    private DomainSearchResponseData(string rawJson)
    {
      _rawJsonResponse = rawJson;
      ParseRawJsonResponse();
    }
    
    private DomainSearchResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    private static List<IFindResponseDomain> GetResponseDomains(JToken domainJson)
    {
      var domains = new List<IFindResponseDomain>();

      if (domainJson != null && domainJson.Any())
      {
        foreach (var domainToken in domainJson)
        {
          var sld = domainToken["NameWithoutExtension"].ToString();
          var tld = domainToken["Extension"].ToString();
          var punyCodeSld = domainToken["PunyCodeNameWithoutExtension"].ToString();
          var punyCodeTld = domainToken["PunyCodeExtension"].ToString();
          var responseDomain = new FindResponseDomain(sld, tld, punyCodeSld, punyCodeTld, domainToken["Data"]) as IFindResponseDomain;

          domains.Add(responseDomain);
        }
      }

      return domains;
    }

    private void ParseRawJsonResponse()
    {
      ExactMatchDomains = new List<IFindResponseDomain>(0);
      Domains = new List<IFindResponseDomain>(0);

      if (!string.IsNullOrEmpty(_rawJsonResponse))
      {
        try
        {
          var reader = new JsonTextReader(new StringReader(_rawJsonResponse));
          var jsonData = new JsonSerializer().Deserialize(reader) as JObject;
          if (jsonData != null)
          {
            ExactMatchDomains = GetResponseDomains(jsonData["ExactDomains"]);
            Domains = GetResponseDomains(jsonData["Domains"]);
          }
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException("DomainSearchResponseData.ParseRawJsonResponse", "0", ex.ToString(), _rawJsonResponse, null, null);
           Engine.Engine.LogAtlantisException(aex);        }
      }
    }
    
    public IList<IFindResponseDomain> ExactMatchDomains { get; private set; }
    public IList<IFindResponseDomain> Domains { get; private set; } 

    public static IResponseData ParseRawResponse(string rawJson)
    {
      return new DomainSearchResponseData(rawJson);
    }

    public static IResponseData FromAtlantisException(AtlantisException exception)
    {
      return new DomainSearchResponseData(exception);
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
