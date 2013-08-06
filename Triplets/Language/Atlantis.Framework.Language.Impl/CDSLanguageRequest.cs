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
      IResponseData result = CDSLanguageResponseData.NotFound;
      var cdsRequestData = requestData as CDSLanguageRequestData;
      
      var wsConfig = (WsConfigElement) config;

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
          var webResponse = (HttpWebResponse) webRequest.GetResponse();
          if (webResponse.StatusCode == HttpStatusCode.OK)
          {
            using (Stream webResponseData = webResponse.GetResponseStream())
            {
              if (webResponseData != null)
              {
                using (var responseReader = new StreamReader(webResponseData))
                {
                  var content = JsonConvert.DeserializeObject<CDSContentVersion>(responseReader.ReadToEnd());
                  responseReader.Close();
                  var dictionary = new PhraseDictionary(false);
                  PhraseDictionary.Parse(dictionary,content.Content, cdsRequestData.DictionaryName,
                                                   cdsRequestData.Language);
                  result = new CDSLanguageResponseData(dictionary);
                }
              }
            }
          }
        }
        catch (WebException ex)
        {
          if (!(ex.Response is HttpWebResponse) || ((HttpWebResponse) ex.Response).StatusCode != HttpStatusCode.NotFound)
          {
            throw;
          }
        }
      }
      return result;
    }
  }
}
