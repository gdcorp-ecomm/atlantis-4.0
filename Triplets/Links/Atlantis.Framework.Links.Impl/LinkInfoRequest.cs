using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Links.Interface;

namespace Atlantis.Framework.Links.Impl
{
  public class LinkInfoRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      string xmlLinkInfo;
      using (var dc = GdDataCacheOutOfProcess.CreateDisposable())
      {
        xmlLinkInfo = dc.GetCacheData(oRequestData.ToXML());
      }

      var linkInfoDoc = new XmlDocument();
      linkInfoDoc.LoadXml(xmlLinkInfo);

      var itemNodes = linkInfoDoc.SelectNodes("//item");
      var linksCollection = new Dictionary<string, ILinkInfo>(itemNodes.Count, StringComparer.OrdinalIgnoreCase);

      foreach (XmlElement itemElement in itemNodes)
      {
        string linkType = itemElement.GetAttribute("linkType");

        LinkTypeLanguageSupport langSuppType;
        Enum.TryParse( itemElement.GetAttribute("languageSupportType"), out langSuppType);

        LinkTypeCountrySupport countrySupportType;
        Enum.TryParse(itemElement.GetAttribute("countrySupportType"), out countrySupportType);
        var item = new LinkInfoImpl
          {
              BaseUrl = itemElement.GetAttribute("baseURL"),
              LanguageSupportType = langSuppType,
              LanguageParameter = itemElement.GetAttribute("languageSupportParam"),
              CountrySupportType = countrySupportType,
              CountryParameter = itemElement.GetAttribute("countrySupportParam")
          };
        linksCollection[linkType] = item; 

      }

      var oGetLinkInfoRequestData = (LinkInfoRequestData)oRequestData;
      if (linksCollection.Count == 0 && oGetLinkInfoRequestData.ContextID != 0)
      {
        string message = String.Concat("Empty LinkInfo exception! ContextId=", oGetLinkInfoRequestData.ContextID);
        throw new Exception(message);
      }

      return new LinkInfoResponseData( linksCollection );
    }

    #endregion

  }
}
