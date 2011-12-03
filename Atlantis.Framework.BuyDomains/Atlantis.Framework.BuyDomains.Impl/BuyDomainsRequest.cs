using System;
using System.IO;
using System.Net;
using Atlantis.Framework.BuyDomains.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyDomains.Impl
{
  public class BuyDomainsRequest : IRequest
  {
    /******************************************************************************/

    #region IRequest Members

    /******************************************************************************/

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      string sResponseXML = "";

      try
      {
        BuyDomainsRequestData oBuyDomainsRequestData = (BuyDomainsRequestData)oRequestData;
        string oRequestURL = ((WsConfigElement)oConfig).WSURL + oBuyDomainsRequestData.GetQuery;
        HttpWebRequest oRequest = (HttpWebRequest)HttpWebRequest.Create(oRequestURL);
        oRequest.Timeout = (int)Math.Truncate(oBuyDomainsRequestData.RequestTimeout.TotalMilliseconds);

        HttpWebResponse oResponse = (HttpWebResponse)oRequest.GetResponse();
        StreamReader srResponse = new StreamReader(oResponse.GetResponseStream());
        sResponseXML = srResponse.ReadToEnd();

        oResponseData = new BuyDomainsResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new BuyDomainsResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new BuyDomainsResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    /******************************************************************************/

    #endregion

    /******************************************************************************/
  }
}
