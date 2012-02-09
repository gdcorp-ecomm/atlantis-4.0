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

      try
      {
        var service = ServiceHelper.GetServiceReference();

        AriesHostingResponse response2 = service.UpdateBandwidthOverageProtection(_AccountGuid.ToString(), bwOp.IsEnabled, bwOp.Suspend);


       
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
