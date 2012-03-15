using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.[TRIPLET].Interface;

namespace Atlantis.Framework.EcommLastOrderLang.Impl
{
  public class [TRIPLET]Request : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      [TRIPLET]ResponseData responseData = null;

      try
      {
        

       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new [TRIPLET]ResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new [TRIPLET]ResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
