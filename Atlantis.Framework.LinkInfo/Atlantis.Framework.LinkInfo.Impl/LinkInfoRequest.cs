using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.LinkInfo.Interface;

namespace Atlantis.Framework.LinkInfo.Impl
{
  public class LinkInfoRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      var dictResult = new Dictionary<string, string>();

      try
      {
        var oGetLinkInfoRequestData = (LinkInfoRequestData)oRequestData;
        string xmlLinkInfo;
        using (var dc = DataCacheService.GdDataCacheOutOfProcess.CreateDisposable())
        {
          xmlLinkInfo = dc.GetCacheData(oRequestData.ToXML());
        }

        var linkInfoDoc = new XmlDocument();
        linkInfoDoc.LoadXml(xmlLinkInfo);

        var itemNodes = linkInfoDoc.SelectNodes("//item");
        foreach (XmlElement itemElement in itemNodes)
        {
          if (itemElement != null)
          {
            string linkType = itemElement.GetAttribute("linkType");
            string baseUrl = itemElement.GetAttribute("baseURL");
            dictResult[linkType] = baseUrl;
          }
        }

        if ((dictResult.Count == 0) && (oGetLinkInfoRequestData.ContextID != 0) && (!oGetLinkInfoRequestData.AllowEmptyLinkSet))
        {
          string message = "Empty LinkInfo exception! ContextId=" + oGetLinkInfoRequestData.ContextID.ToString();
          throw new Exception(message);
        }

        oResponseData = new LinkInfoResponseData(dictResult);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new LinkInfoResponseData(dictResult, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new LinkInfoResponseData(dictResult, oRequestData, ex);
      }

      return oResponseData;

    }

    #endregion

  }
}
