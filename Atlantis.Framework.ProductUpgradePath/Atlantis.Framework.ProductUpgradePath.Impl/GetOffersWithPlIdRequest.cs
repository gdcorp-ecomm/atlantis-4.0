using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductUpgradePath.Interface;

namespace Atlantis.Framework.ProductUpgradePath.Impl
{
  public class ProductUpgradePathRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ProductUpgradePathResponseData responseData = null;

      try
      {
        

       
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new ProductUpgradePathResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new ProductUpgradePathResponseData(requestData, ex);
      }
       
      return responseData;
    }
  }
}
