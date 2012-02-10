using System;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDUpdFTPBackupInfo.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDUpdFTPBackupInfo.Impl
{
  public class HDVDUpdFTPBackupInfoRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      HDVDUpdFTPBackupInfoResponseData responseData = null;

      var request = requestData as HDVDUpdFTPBackupInfoRequestData;

      try
      {
        HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);
        AriesHostingResponse response = service.UpdateFTPBackupInformation(request.AccountUid, request.Username, request.Password);
               
        responseData = new HDVDUpdFTPBackupInfoResponseData(response);

       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new HDVDUpdFTPBackupInfoResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HDVDUpdFTPBackupInfoResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
