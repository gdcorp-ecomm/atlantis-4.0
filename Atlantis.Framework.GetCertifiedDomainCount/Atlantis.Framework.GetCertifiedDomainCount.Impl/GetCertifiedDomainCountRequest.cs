using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GetCertifiedDomainCount.Interface;
using Atlantis.Framework.GetCertifiedDomainCount.Impl.CDCustomerWebSvc;

namespace Atlantis.Framework.GetCertifiedDomainCount.Impl
{
  public class GetCertifiedDomainCountRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GetCertifiedDomainCountResponseData responseData = null;

      try
      {
        int certCount = 0;
        GetCertifiedDomainCountRequestData domainCountRequestData = (GetCertifiedDomainCountRequestData)requestData;
        CDCustomerWebSvc.CDCustomerWebSvc domainCountWS = new CDCustomerWebSvc.CDCustomerWebSvc();
        domainCountWS.Url = ((WsConfigElement)config).WSURL;
        domainCountWS.Timeout = (int)domainCountRequestData.RequestTimeout.TotalMilliseconds;
        LongResults wsResponse = domainCountWS.GetCertifiedDomainCount(config.GetConfigValue("ApplicationName"), domainCountRequestData.ShopperID);

        if (wsResponse != null)
        {
          if (wsResponse.Status == ProcessingResults.SUCCESS)
          {
            certCount = (int)wsResponse.Results;
            responseData = new GetCertifiedDomainCountResponseData(certCount);
          }
          else
          {
            AtlantisException aex = new AtlantisException(domainCountRequestData
              , "GetCertifiedDomainCount::RequestHandler"
              , wsResponse.Error.Message
              , string.Format("Code: {0} | Detail {1}", wsResponse.Error.Code, wsResponse.Error.Detail));

            responseData = new GetCertifiedDomainCountResponseData(aex);
          }
        }
        else
        {
          AtlantisException aex = new AtlantisException(domainCountRequestData
            , "GetCertifiedDomainCount::RequestHandler"
            , "Null Response from Web Service call"
            , string.Empty);

          responseData = new GetCertifiedDomainCountResponseData(aex);
        }
      }     
      catch (AtlantisException exAtlantis)
      {
        responseData = new GetCertifiedDomainCountResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new GetCertifiedDomainCountResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
