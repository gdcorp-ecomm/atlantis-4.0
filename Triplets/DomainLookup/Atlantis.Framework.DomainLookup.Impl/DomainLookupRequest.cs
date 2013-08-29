using Atlantis.Framework.DomainLookup.Interface;
using Atlantis.Framework.Interface;
using System.Data;
using System;

namespace Atlantis.Framework.DomainLookup.Impl
{

  public class DomainLookupRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DomainLookupResponseData dlResponseData;
     
      try
      {
        DomainLookupRequestData dlRequestData = (DomainLookupRequestData)requestData;

        if (string.IsNullOrEmpty(dlRequestData.DomainName))
        {
          throw new Exception("Domain name was passed as empty.");
        }

        DomainLookupWS.ParkWeb dlWebService = new DomainLookupWS.ParkWeb();
        dlWebService.Url = ((WsConfigElement)config).WSURL;
        dlWebService.Timeout = (int)dlRequestData.RequestTimeout.TotalMilliseconds;

        DataSet dataSet = dlWebService.GetDomainInfoWithPdDomain(dlRequestData.DomainName);
        dlResponseData = DomainLookupResponseData.FromData(dataSet);
      }
      catch (Exception ex)
      {
        AtlantisException atlantisEx = new AtlantisException(requestData, "Atlantis.Framework.DomainLookup.Impl::RequestHandler", ex.Message, ex.StackTrace);
        dlResponseData = DomainLookupResponseData.FromData(atlantisEx);
      }


      return dlResponseData;
    }
  }
}
