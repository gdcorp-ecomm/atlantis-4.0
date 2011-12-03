using System;
using System.Collections.Generic;
using Atlantis.Framework.CreateIncidentInIRIS.Interface;
using Atlantis.Framework.CreateIncidentInIRIS.Impl.IrisWS;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CreateIncidentInIRIS.Impl
{
  public class CreateIncidentInIRISRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      CreateIncidentInIRISResponseData oResponseData = null;

      IrisWS.IrisWebService service = null;
      try
      {
        CreateIncidentInIRISRequestData request = (CreateIncidentInIRISRequestData)oRequestData;
        service = new IrisWebService();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        service.Timeout = (int)Math.Truncate(request.RequestTimeout.TotalMilliseconds);

        long irisResult = service.CreateIncidentInIRIS(request.SubscriberId,
                                                       request.Subject,
                                                       request.Note,
                                                       request.CustomerEmailAddress,
                                                       request.OriginalIPAddress,
                                                       request.GroupId,
                                                       request.ServiceId,
                                                       request.PrivateLabelId,
                                                       request.ShopperId,
                                                       request.CreatedBy);

        oResponseData = new CreateIncidentInIRISResponseData(irisResult);

      }
      catch (Exception ex)
      {
        oResponseData = new CreateIncidentInIRISResponseData(oRequestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }

      return oResponseData;
    }

    #endregion
    
  }
}
