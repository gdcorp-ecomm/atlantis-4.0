using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Impl
{
  public class TLDLanguageRequest : IRequest
  {
    private const string _TLDLANGUAGEINFO_REQUESTFORMAT = "<GetLanguageListByTLDId><param name=\"tldId\" value=\"{0}\"/></GetLanguageListByTLDId>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        TLDLanguageRequestData languageRequest = (TLDLanguageRequestData)requestData;
        string requestXml = string.Format(_TLDLANGUAGEINFO_REQUESTFORMAT, languageRequest.TLDId.ToString());
        string responseXml;

        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          responseXml = comCache.GetCacheData(requestXml);
        }

        XElement tldElements = XElement.Parse(responseXml);
        result = TLDLanguageResponseData.FromDataCacheElement(tldElements);
      }
      catch (Exception ex)
      {
        result = TLDLanguageResponseData.FromException(requestData, ex);
      }

      return result;
    }
  }
}
