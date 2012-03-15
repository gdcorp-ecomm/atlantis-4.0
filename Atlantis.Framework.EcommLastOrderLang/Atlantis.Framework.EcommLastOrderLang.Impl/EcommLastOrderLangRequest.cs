using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommLastOrderLang.Interface;

namespace Atlantis.Framework.EcommLastOrderLang.Impl
{
  public class EcommLastOrderLangRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommLastOrderLangResponseData responseData = null;

      try
      {
        

       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new EcommLastOrderLangResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EcommLastOrderLangResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
