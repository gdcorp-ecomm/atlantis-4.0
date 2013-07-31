using Atlantis.Framework.DomainLookup.Interface;
using Atlantis.Framework.Interface;
using System.Data;
using System;

namespace Atlantis.Framework.DomainLookup.Impl
{
    // Possible atlantis.config entry - remove this before peer review
    // <ConfigElement progid="Atlantis.Framework.DomainLookup.Impl.DomainLookup" assembly="Atlantis.Framework.DomainLookup.Impl.dll" request_type="###" />

    public class DomainLookupRequest : IRequest
    {
        public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
        {
            DomainLookupResponseData dlResponseData = null;
            DataSet ds = null;

            // Handle the request and return the IResponseData object for the request
            // Returning null will cause an exception
            try
            {
                DomainLookupRequestData dlRequestData = (DomainLookupRequestData)requestData;

                DomainLookupWS.ParkWeb dlWebService = new DomainLookupWS.ParkWeb();
                dlWebService.Url = ((WsConfigElement)config).WSURL;
                dlWebService.Timeout = (int)dlRequestData.RequestTimeout.TotalMilliseconds;
                ds = dlWebService.GetDomainInfo(dlRequestData.DomainName);
                dlResponseData = DomainLookupResponseData.FromData(ds, null);
            }
            catch (AtlantisException atlantisEx)
            {
                dlResponseData = DomainLookupResponseData.FromData(ds, atlantisEx);
            }
            catch (Exception ex)
            {
                AtlantisException atlantisEx = new AtlantisException(requestData, "Atlantis.Framework.DomainLookup.Impl::RequestHandler", ex.Message, ex.StackTrace);
                dlResponseData = DomainLookupResponseData.FromData(ds, atlantisEx);
            }


            return dlResponseData;
        }
    }
}
