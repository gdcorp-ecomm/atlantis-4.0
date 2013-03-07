using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Impl
{
  public class ExtendedTLDDataRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        string tld = ((ExtendedTLDDataRequestData)requestData).TLD;
        if (string.IsNullOrEmpty(tld))
        {
          throw new ArgumentException("TLD cannot be empty or null.");
        }

        string responseXml;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          responseXml = comCache.GetTLDData(tld);
        }

        XElement tldElements = XElement.Parse(responseXml);
        result = ExtendedTLDDataResponseData.FromDataCacheElement(tldElements);
      }
      catch (Exception ex)
      {
        result = ExtendedTLDDataResponseData.FromException(requestData, ex);
      }

      return result;
    }
  }
}
