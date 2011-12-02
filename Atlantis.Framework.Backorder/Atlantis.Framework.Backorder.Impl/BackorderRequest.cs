using System;
using Atlantis.Framework.Backorder.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Backorder.Impl
{
  public class BackorderRequest : IRequest
  {
    /******************************************************************************/

    #region IRequest Members

    /******************************************************************************/

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      string sResponseXML = "";

      RegChkIsBackorder.RegChkIsBackorderService oRequest = null;
      try
      {
        BackorderRequestData oBackOrderRequestData = (BackorderRequestData)oRequestData;
        
        oRequest = new RegChkIsBackorder.RegChkIsBackorderService();
        oRequest.Url = ((WsConfigElement)oConfig).WSURL;
        oRequest.Timeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalMilliseconds);

        sResponseXML = oRequest.IsMultipleDomainBackorderAllowed(oBackOrderRequestData.ToXML());

        if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                               "BackorderRequest.RequestHandler",
                                                               sResponseXML,
                                                               oRequestData.ToXML());

          oResponseData = new BackorderResponseData(sResponseXML, exAtlantis);
        }
        else
        {
          oResponseData = new BackorderResponseData(sResponseXML);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new BackorderResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new BackorderResponseData(sResponseXML, oRequestData, ex);
      }
      finally
      {
        if (oRequest!=null)
        {
          oRequest.Dispose();
        }
      }

      return oResponseData;
    }

    /******************************************************************************/

    #endregion

    /******************************************************************************/
  }
}
