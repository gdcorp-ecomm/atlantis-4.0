using System;
using System.IO;
using System.Net;
using Atlantis.Framework.FabDomains.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FabDomains.Impl
{
  public class FabDomainsRequest : IRequest
  {
    /******************************************************************************/

    #region IRequest Members

    /******************************************************************************/

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {

      IResponseData oResponseData = null;
      string sResponseXML = String.Empty;

      try
      {
        FabDomainsRequestData oFabDomainsRequestData = (FabDomainsRequestData)oRequestData;

        string oRequestURL = ((WsConfigElement)oConfig).WSURL + oFabDomainsRequestData.GetQuery;

        HttpWebRequest oRequest = (HttpWebRequest)HttpWebRequest.Create(oRequestURL);
        oRequest.Timeout = (int)Math.Truncate(oFabDomainsRequestData.RequestTimeout.TotalMilliseconds);

        HttpWebResponse oResponse = (HttpWebResponse)oRequest.GetResponse();
        StreamReader srResponse = new StreamReader(oResponse.GetResponseStream());
        sResponseXML = srResponse.ReadToEnd();

        oResponseData = new FabDomainsResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new FabDomainsResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new FabDomainsResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    /******************************************************************************/

    #endregion

    /******************************************************************************/
  }
}
