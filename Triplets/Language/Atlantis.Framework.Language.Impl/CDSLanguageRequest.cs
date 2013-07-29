using Atlantis.Framework.Interface;
using Atlantis.Framework.Language.Impl.Data;
using Atlantis.Framework.Language.Interface;
using Atlantis.Framework.Parsers.LanguageFile;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace Atlantis.Framework.Language.Impl
{
  public class CdsLanguageRequest : IRequest
  {
    const string LocalizationUrl = "{0}content/localization/{1}/{2}";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      var cdsRequestData = requestData as CDSLanguageRequestData;
      var cdsResponseData = new CDSLanguageResponseData
        {
          Exception = null,
          Phrases = new Parsers.LanguageFile.PhraseDictionary(),
          Exists = false,
          StatusCode = HttpStatusCode.NotFound
        };
      var wsConfig = (WsConfigElement) config;
      HttpWebResponse webResponse = null;

      if (cdsRequestData != null)
      {
        try
        {
          var webRequest =
            (HttpWebRequest)
            WebRequest.Create(string.Format(LocalizationUrl, wsConfig.WSURL, cdsRequestData.DictionaryName,
                                            cdsRequestData.Language));
          webRequest.Method = WebRequestMethods.Http.Get;
          webRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
          webResponse = (HttpWebResponse) webRequest.GetResponse();
          if (webResponse.StatusCode == HttpStatusCode.OK)
          {
            return ValidResponse(webResponse, cdsResponseData, cdsRequestData.DictionaryName,
                                 cdsRequestData.Language);
          }
        }
        catch (WebException)
        {
          if (webResponse != null && webResponse.StatusCode == HttpStatusCode.NotFound)
          {
            return cdsResponseData;
          }
        }
        catch (Exception ex)
        {
          if (webResponse != null)
          {
            cdsResponseData.StatusCode = webResponse.StatusCode;
          }
          cdsResponseData.Exception = new AtlantisException(cdsRequestData, ex.Source, ex.Message, string.Empty, ex);
        }
      }

      return cdsResponseData;
    }

    private static CDSLanguageResponseData ValidResponse(HttpWebResponse webResponse, CDSLanguageResponseData cdsResponseData, string dictionaryName, string language)
    {
      using (Stream webResponseData = webResponse.GetResponseStream())
      {
        if (webResponseData != null)
        {
          using (var responseReader = new StreamReader(webResponseData))
          {
            var content = JsonConvert.DeserializeObject<CDSContentVersion>(responseReader.ReadToEnd());
            cdsResponseData.Phrases = LanguageFile.Parse(content.Content, dictionaryName, language);
            cdsResponseData.Exists = true;
            cdsResponseData.StatusCode = webResponse.StatusCode;
            responseReader.Close();
          }
        }
      }
      return cdsResponseData;
    }
  }
}
