using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;
using Atlantis.Framework.SA.Interface.Authorization;
using Atlantis.Framework.SAGetDomainListByShopper.Interface;

namespace Atlantis.Framework.SAGetDomainListByShopper.Impl
{
  public class SAGetDomainListByShopperRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      SAGetDomainListByShopperResponseData responseData;
      var request = requestData as SAGetDomainListByShopperRequestData;

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

        var data = ws.GetDomainListByShopper(authName, request.ShopperID);

        if (data != null)
        {
          //convert data from service definition type to SA friendly datatype
          var domainList = new DomainListResponseData
                             {
                               DomainCount = data.domainCount,
                               ReturnCode = data.returnCode,
                               ReturnMessage = data.returnMessage
                             };

          foreach (var domain in data.domainList)
          {
            domainList.DomainList.Add(domain.Domain);
          }
          
          //return a response based on friendly datatype
          responseData = new SAGetDomainListByShopperResponseData(domainList);
        }
        else
        {
          var aex = new AtlantisException(request, "SAGetDomainListByShopper.RequestHandler", "API Response is null or domain list data is null",string.Empty);
          responseData = new SAGetDomainListByShopperResponseData(request, aex);
        }

      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new SAGetDomainListByShopperResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new SAGetDomainListByShopperResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
