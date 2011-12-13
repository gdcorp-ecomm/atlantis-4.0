using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSCSetupAccount.Interface;
using Atlantis.Framework.QSCSetupAccount.Impl.QSCService;

namespace Atlantis.Framework.QSCSetupAccount.Impl
{
  public class QSCSetupAccountRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      QSCSetupAccountResponseData responseData = null;

      try
      {
        QSCSetupAccountRequestData request = (QSCSetupAccountRequestData)requestData;
        QSCService.FOSService service = new QSCService.FOSService();
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        acountSetup result = service.setupAccount(request.AccountUID, request.DomainName,request.EmailAddress,request.CompanyName,request.ThemeID);
        responseData = new QSCSetupAccountResponseData(result.responseCode, result.responseMessage);
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new QSCSetupAccountResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new QSCSetupAccountResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
