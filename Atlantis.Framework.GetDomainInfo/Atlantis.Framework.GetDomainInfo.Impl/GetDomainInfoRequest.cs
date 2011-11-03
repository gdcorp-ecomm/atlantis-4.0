using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Atlantis.Framework.Interface;
using Atlantis.Framework.GetDomainInfo.Interface;
using System.Xml;

namespace Atlantis.Framework.GetDomainInfo.Impl
{
  public class GetDomainInfoRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetDomainInfoRequestData getDomainInfoRequest = oRequestData as GetDomainInfoRequestData;
      GetDomainInfoResponseData getDomainInfoResponse = null;

      DomainInfoWS.RegCheckDomainStatusWebSvcService regCheckService = new DomainInfoWS.RegCheckDomainStatusWebSvcService();
      regCheckService.Url = ((WsConfigElement)oConfig).WSURL;

      try
      {
        string requestXML = getDomainInfoRequest.ToXML();
        string response = regCheckService.GetDomainInfoByNameWithContacts(requestXML);

        if (response != null)
        {
          getDomainInfoResponse = new GetDomainInfoResponseData(response);
        }
        else
        { 
          //Unknown error
          getDomainInfoResponse = new GetDomainInfoResponseData(getDomainInfoRequest, new Exception("Unknown error occurred in the GetDomainInfoByNameWithContacts webmethod. ResultCode is null."));
        }
      }
      catch (AtlantisException exAtlantis)
      {
        getDomainInfoResponse = new GetDomainInfoResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        getDomainInfoResponse = new GetDomainInfoResponseData(oRequestData, ex);
      }

      return getDomainInfoResponse;
    }
  }
}

