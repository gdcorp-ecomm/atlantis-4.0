using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Impl
{
  public class ActiveTLDsRequest : IRequest
  {
    private const string _TLDINFOREQUESTFORMAT = "<GetTLDInfo><param name=\"tldIdOrName\" value=\"{0}\"/></GetTLDInfo>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        string tld = ((ActiveTLDsRequestData)requestData).TLD;
        if (string.IsNullOrEmpty(tld))
        {
          throw new ArgumentException("TLD cannot be empty or null.");
        }

        string requestXml = string.Format(_TLDINFOREQUESTFORMAT, tld);

        string responseXml;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          responseXml = comCache.GetCacheData(requestXml);
        }

        XElement tldElements = XElement.Parse(responseXml);
        result = ActiveTLDsResponseData.FromDataCacheElement(tldElements);
      }
      catch (Exception ex)
      {
        result = ActiveTLDsResponseData.FromException(requestData, ex);
      }

      return result;
    }
  }
}
