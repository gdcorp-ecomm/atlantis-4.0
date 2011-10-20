using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RegVendorDomainSearch.Interface;
using Atlantis.Framework.RegVendorDomainSearch.Impl.gdDPPSearch;

namespace Atlantis.Framework.RegVendorDomainSearch.Impl
{
  class RegVendorDomainSearchAsyncRequest : IAsyncRequest
  {
    /******************************************************************************/

    #region IAsyncRequest Members

    /******************************************************************************/

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      RegVendorDomainSearchRequestData requestData = (RegVendorDomainSearchRequestData)oRequestData;

      gdDppSearchWS oRequest = new gdDppSearchWS();
      oRequest.Url = ((WsConfigElement)oConfig).WSURL;

      AsyncState oAsyncState = new AsyncState(oRequestData, oConfig, oRequest, oState);

      IAsyncResult oAsyncResult = oRequest.BegindppDomainSearch(requestData.ToXML(), oCallback, oAsyncState);
      return oAsyncResult;
    }

    /******************************************************************************/

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      RegVendorDomainSearchResponseData responseData = null;
      AsyncState oAsyncState = (AsyncState)oAsyncResult.AsyncState;
      string responseXml = string.Empty;

      try
      {
        gdDppSearchWS request = (gdDppSearchWS)oAsyncState.Request;
        responseXml = request.EnddppDomainSearch(oAsyncResult);
        responseData = new RegVendorDomainSearchResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RegVendorDomainSearchResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RegVendorDomainSearchResponseData(responseXml, oAsyncState.RequestData, ex);
      }

      return responseData;

    }
    
    /******************************************************************************/

    #endregion

  }
}
