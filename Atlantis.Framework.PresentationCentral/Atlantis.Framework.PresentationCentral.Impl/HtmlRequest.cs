using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PresentationCentral.Interface;

namespace Atlantis.Framework.PresentationCentral.Impl
{
  public class HtmlRequest : IRequest
  {
    // **************************************************************** //

    #region IRequest Members

    // **************************************************************** //

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      HtmlResponseData oResponseData = null;

      try
      {
        HtmlRequestData oPageHeaderData = (HtmlRequestData)oRequestData;

        PresentationCentral.PresentationCentral oPresentationCentralWS = new PresentationCentral.PresentationCentral();
        oPresentationCentralWS.Url = ((WsConfigElement)oConfig).WSURL;
        oPresentationCentralWS.Timeout = (int)oPageHeaderData.RequestTimeout.TotalMilliseconds;
        string sRequestXML = oRequestData.ToXML();

        XmlNode oResultNode = oPresentationCentralWS.RequestHTML(sRequestXML);

        if (oResultNode == null) // Bad request yields a null XmlNode
        {
          throw new AtlantisException((RequestData)oRequestData,
                                      "PageHeader.RequestHandler",
                                      "Invalid request, (null) node returned",
                                      oRequestData.ToXML());
        }

        oResponseData = new HtmlResponseData(oResultNode);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new HtmlResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new HtmlResponseData((HtmlRequestData)oRequestData, ex);
      }

      return oResponseData;
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //

  }
}
