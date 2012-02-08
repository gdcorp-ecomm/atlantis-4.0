using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.HDVDSubmitReprovision.Interface;

namespace Atlantis.Framework.HDVDSubmitReprovision.Impl
{
  public class HDVDSubmitReprovisionRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      HDVDSubmitReprovisionResponseData responseData = null;

      try
      {
        

       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new HDVDSubmitReprovisionResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new HDVDSubmitReprovisionResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
