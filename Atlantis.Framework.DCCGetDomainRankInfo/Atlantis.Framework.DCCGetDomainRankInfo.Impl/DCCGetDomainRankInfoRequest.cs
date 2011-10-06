using System;
using Atlantis.Framework.DCCGetDomainRankInfo.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDomainRankInfo.Impl
{
  public class DCCGetDomainRankInfoRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DCCGetDomainRankInfoResponseData responseData = null;

      try
      {
        DCCGetDomainRankInfoRequestData request = (DCCGetDomainRankInfoRequestData)requestData;

        WebScoreBossWebSvc.WebScoreBossWebSvc domainRankInfoWS = new WebScoreBossWebSvc.WebScoreBossWebSvc();
        domainRankInfoWS.Url = ((WsConfigElement)config).WSURL;
        domainRankInfoWS.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        string wsResponseXml = domainRankInfoWS.GetRankInfoForDomainIds(request.ToXML());

        if (!string.IsNullOrEmpty(wsResponseXml) && wsResponseXml.Contains("<success>1</success>"))
        {
          responseData = new DCCGetDomainRankInfoResponseData(wsResponseXml);
        }
        else
        {
          responseData = new DCCGetDomainRankInfoResponseData(request, wsResponseXml);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCGetDomainRankInfoResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new DCCGetDomainRankInfoResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
