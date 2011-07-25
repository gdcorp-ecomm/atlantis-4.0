using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Atlantis.Framework.Interface;
using Atlantis.Framework.PresentationCentral.Interface;

namespace Atlantis.Framework.PresentationCentral.Impl
{
  public class HtmlAsyncRequest : IAsyncRequest
  {

    // **************************************************************** //

    #region IAsyncRequest Members

    // **************************************************************** //

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      HtmlRequestData oHtmlRequestData = (HtmlRequestData)oRequestData;
      using (PresentationCentral.PresentationCentral oPresentationCentralWS = new PresentationCentral.PresentationCentral())
      {
        oPresentationCentralWS.Url = ((WsConfigElement)oConfig).WSURL;
        string sRequestXML = oHtmlRequestData.ToXML();
        AsyncState oAsyncState = new AsyncState(oRequestData, oConfig, oPresentationCentralWS, oState);

        return oPresentationCentralWS.BeginRequestHTML(oRequestData.ToXML(), oCallback, oAsyncState);
      }
    }

    // **************************************************************** //

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      IResponseData oResponseData = null;

      AsyncState oAsyncState = (AsyncState)oAsyncResult.AsyncState;
      try
      {
		  PresentationCentral.PresentationCentral oPresentationCentralWS = (PresentationCentral.PresentationCentral)oAsyncState.Request;
        XmlNode oResultNode = oPresentationCentralWS.EndRequestHTML(oAsyncResult);

        if (oResultNode == null) // Bad request yields a null XmlNode
        {
          throw new AtlantisException((RequestData)oAsyncState.RequestData,
                                      "PageHeader.EndHandleRequest",
                                      "Invalid request, (null) node returned",
                                      oAsyncState.RequestData.ToXML());
        }

        oResponseData = new HtmlResponseData(oResultNode);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new HtmlResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new HtmlResponseData(oAsyncState.RequestData, ex);
      }

      return oResponseData;
    }
    // **************************************************************** //

    #endregion

    // **************************************************************** //

  }
  
}
