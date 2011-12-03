using System;
using System.IO;
using System.Net;
using Atlantis.Framework.BuyDomains.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BuyDomains.Impl
{
  public class BuyDomainsAsyncRequest : IAsyncRequest
  {
    /******************************************************************************/

    #region IAsyncRequest Members

    /******************************************************************************/

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      BuyDomainsRequestData oBuyDomainsRequestData = (BuyDomainsRequestData)oRequestData;

      string oRequestURL = ((WsConfigElement)oConfig).WSURL + oBuyDomainsRequestData.GetQuery;

      HttpWebRequest oRequest = (HttpWebRequest)HttpWebRequest.Create(oRequestURL);

      AsyncState oAsyncState = new AsyncState(oRequestData, oConfig, oRequest, oState);

      IAsyncResult oAsyncResult = oRequest.BeginGetResponse(oCallback, oAsyncState);

      return oAsyncResult;
    }

    /******************************************************************************/

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      IResponseData oResponseData = null;
      string sResponseXML = "";
      AsyncState oAsyncState = (AsyncState)oAsyncResult.AsyncState;

      try
      {
        HttpWebRequest oRequest = (HttpWebRequest)oAsyncState.Request;
        HttpWebResponse oResponse = (HttpWebResponse)oRequest.EndGetResponse(oAsyncResult);
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
        oResponseData = new BuyDomainsResponseData(sResponseXML, oAsyncState.RequestData, ex);
      }

      return oResponseData;
    }

    /******************************************************************************/

    #endregion

    /******************************************************************************/
  }
}
