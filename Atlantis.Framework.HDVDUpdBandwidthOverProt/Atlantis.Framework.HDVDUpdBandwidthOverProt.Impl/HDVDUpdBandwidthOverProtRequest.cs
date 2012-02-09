using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.Interface;
using Atlantis.Framework.HDVDUpdBandwidthOverProt.Interface;

namespace Atlantis.Framework.HDVDUpdBandwidthOverProt.Impl
{
  public class HDVDUpdBandwidthOverProtRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      HDVDUpdBandwidthOverProtResponseData responseData = null;
      var request = requestData as HDVDUpdBandwidthOverProtRequestData;

      try
      {
        HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

        AriesHostingResponse response = service.UpdateBandwidthOverageProtection(request.AccountUid, request.IsEnabled, request.Suspend);

        responseData = new HDVDUpdBandwidthOverProtResponseData(response);


      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new HDVDUpdBandwidthOverProtResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HDVDUpdBandwidthOverProtResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
