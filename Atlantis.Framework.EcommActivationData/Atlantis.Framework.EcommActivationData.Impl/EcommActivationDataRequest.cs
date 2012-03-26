using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommActivationData.Interface;

namespace Atlantis.Framework.EcommActivationData.Impl
{
  public class EcommActivationDataRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommActivationDataResponseData responseData = null;

      try
      {
        

       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new EcommActivationDataResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EcommActivationDataResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
