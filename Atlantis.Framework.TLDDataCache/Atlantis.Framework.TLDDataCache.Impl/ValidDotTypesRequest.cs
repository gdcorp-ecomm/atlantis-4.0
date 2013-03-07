using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Impl
{
  public class ValidDotTypesRequest : IRequest
  {
    private const string _TLDINFOREQUESTFORMAT = "<GetTLDList />";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        string responseXml;

        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          responseXml = comCache.GetCacheData(_TLDINFOREQUESTFORMAT);
        }

        XElement tldElements = XElement.Parse(responseXml);
        result = ValidDotTypesResponseData.FromDataCacheElement(tldElements);
      }
      catch (Exception ex)
      {
        result = ValidDotTypesResponseData.FromException(requestData, ex);
      }

      return result;
    }
  }
}
