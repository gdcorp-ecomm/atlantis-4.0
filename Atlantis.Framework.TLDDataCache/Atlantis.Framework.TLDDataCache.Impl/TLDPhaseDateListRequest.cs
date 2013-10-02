using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Impl
{
  public class TLDPhaseDateListRequest : IRequest
  {
    private const string REQUESTFORMAT = "<TLDPhaseDateGetList><param name=\"n_tldID\" value=\"{0}\"/></TLDPhaseDateGetList>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        int tldId = ((TLDPhaseDateListRequestData)requestData).TldId;
        if (tldId <= 0)
        {
          throw new ArgumentException("TldId must be greater than zero.");
        }

        string requestXml = string.Format(REQUESTFORMAT, tldId);

        string responseXml = String.Empty;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          responseXml = comCache.GetCacheData(requestXml);
        }

        //responseXml = "<data count=\"1\"><item gdshop_tldPhase=\"LR\" phaseStartDate=\"04/22/2013\" phaseEndDate=\"12/22/2014\" /></data>";
        XElement listElements = XElement.Parse(responseXml);

        result = TLDPhaseDateListResponseData.FromDataCacheElement(listElements);
      }
      catch (Exception ex)
      {
        result = TLDPhaseDateListResponseData.FromException(requestData, ex);
      }

      return result;
    }
  }
}
