using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.Interface;
using Atlantis.Framework.HDVDValidateFTPBackupInfo.Interface;

namespace Atlantis.Framework.HDVDValidateFTPBackupInfo.Impl
{
  public class HDVDValidateFTPBackupInfoRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      HDVDValidateFTPBackupInfoResponseData responseData = null;

      try
      {     
        HDVDValidateFTPBackupInfoRequestData request = requestData as HDVDValidateFTPBackupInfoRequestData;

        HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);
        AriesValidationResponse response = service.ValidateFormUpdateFTPBackupInformation(request.AccountUid, request.Username, request.Password);
   
        responseData = new HDVDValidateFTPBackupInfoResponseData(response);
       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new HDVDValidateFTPBackupInfoResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HDVDValidateFTPBackupInfoResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
