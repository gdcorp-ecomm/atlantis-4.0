using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.Interface;
using Atlantis.Framework.HDVDGetBandwidthUsage.Interface;

namespace Atlantis.Framework.HDVDGetBandwidthUsage.Impl
{
  public class HDVDGetBandwidthUsageRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      HDVDGetBandwidthUsageResponseData responseData = null;

      try
      {
        HDVDGetBandwidthUsageRequestData request = requestData as HDVDGetBandwidthUsageRequestData;

        HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);
        AriesBandwidthUsageResponse response = service.GetBandwidthUsage(request.AccountUid);

        responseData = new HDVDGetBandwidthUsageResponseData(response);

       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new HDVDGetBandwidthUsageResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HDVDGetBandwidthUsageResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
