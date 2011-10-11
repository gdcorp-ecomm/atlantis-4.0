using System;
using Atlantis.Framework.DCCIsDomainAlertCancellable.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCIsDomainAlertCancellable.Impl
{
  public class DCCIsDomainAlertCancellableRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DCCIsDomainAlertCancellableResponseData responseData;
      RegBackorderDataWebSvc.RegBackorderDataWebSvc regBackorderDataWebSvc = null;

      try
      {
        DCCIsDomainAlertCancellableRequestData request = (DCCIsDomainAlertCancellableRequestData)requestData;
        regBackorderDataWebSvc = new RegBackorderDataWebSvc.RegBackorderDataWebSvc();
        regBackorderDataWebSvc.Url = ((WsConfigElement)config).WSURL;
        regBackorderDataWebSvc.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        string wsResponseXml = regBackorderDataWebSvc.IsDomainAlertCancellable(request.ToXML());

        if (wsResponseXml.Contains("error method="))
        {
          responseData = new DCCIsDomainAlertCancellableResponseData(request, wsResponseXml);
        }
        else
        {
          responseData = new DCCIsDomainAlertCancellableResponseData(wsResponseXml);
        }
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCIsDomainAlertCancellableResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new DCCIsDomainAlertCancellableResponseData(requestData, ex);
      }
      finally
      {
        if(regBackorderDataWebSvc != null)
        {
          regBackorderDataWebSvc.Dispose();
        }
      }
       
      return responseData;
    }
  }
}
