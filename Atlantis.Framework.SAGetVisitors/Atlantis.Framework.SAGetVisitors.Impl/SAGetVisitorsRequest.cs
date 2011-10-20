using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.SA.Interface.Authorization;
using Atlantis.Framework.SAGetVisitors.Interface;

namespace Atlantis.Framework.SAGetVisitors.Impl
{
  public class SAGetVisitorsRequest :IRequest 
  {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
       SAGetVisitorsResponseData responseData;
       var request = requestData as SAGetVisitorsRequestData;

      try
      {
        // corp.web.mya.sa.api 
        string  authName, authToken;
        string nimitzAuthXml = NetConnect.LookupConnectInfo(config, ConnectLookupType.Xml);
        NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out authName, out authToken);
        
        var ws = new SAMobileWeb.SAMobileWeb
                   {
                     Url = ((WsConfigElement) config).WSURL,
                     Timeout = (request == null) ? (int)(new TimeSpan(0,0,0,30)).TotalMilliseconds :  
                                                   (int) request.RequestTimeout.TotalMilliseconds
                   };

        var data = ws.Visitors(authName, request.Domain, request.StartDate.ToShortDateString(), request.EndDate.ToShortDateString());

        if (data != null)
        {
          //convert data from service definition type to SA friendly datatype
          var visitData = new VisitorResponseData
                             {
                               ShopperId =  data.shopper,
                               ReturnCode = data.returnCode,
                               ReturnMessage = data.returnMessage
                             };

          foreach (var visit in data.visitorStats)
          {
            visitData.Visits.Add(new Visit(visit.StatsDate, visit.UniqueVisitors, visit.Visitors));
          }
          
          //return a response based on friendly datatype
          responseData = new SAGetVisitorsResponseData(visitData);
        }
        else
        {
          var aex = new AtlantisException(request, "SAGetVisitorRequest.RequestHandler", "API Response is null or domain list data is null", string.Empty);
          responseData = new SAGetVisitorsResponseData(request, aex);
        }

      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new SAGetVisitorsResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new SAGetVisitorsResponseData(requestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}
