using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AuthRetrieve.Interface;

namespace Atlantis.Framework.AuthRetrieve.Impl
{
  public class AuthRetrieveRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthRetrieveResponseData responseData = null;

      try
      {
        

       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuthRetrieveResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new AuthRetrieveResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
