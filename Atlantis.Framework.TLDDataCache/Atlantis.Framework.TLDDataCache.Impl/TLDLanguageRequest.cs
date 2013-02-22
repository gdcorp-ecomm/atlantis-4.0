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
        int tldId = ((TLDLanguageRequestData)requestData).TLDId;
        if (tldId == 0)
        {
          throw new ArgumentException("TLD cannot be empty or null.");
        }

        string requestXml = string.Format(_TLDLANGUAGEINFO_REQUESTFORMAT, tldId);
        string responseXml = DataCache.DataCache.GetCacheData(requestXml);
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
