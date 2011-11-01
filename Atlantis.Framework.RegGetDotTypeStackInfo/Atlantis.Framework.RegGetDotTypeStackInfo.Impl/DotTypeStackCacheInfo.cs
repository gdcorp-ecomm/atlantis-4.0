using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RegGetDotTypeStackInfo.Interface;

namespace Atlantis.Framework.RegGetDotTypeStackInfo.Impl
{
  public class DotTypeStackCacheInfo : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      Dictionary<string, Dictionary<string, DotTypeStackItem>> _dotTypeStackItems = new Dictionary<string, Dictionary<string, DotTypeStackItem>>();
      string xml = string.Empty;

      try
      {
        RegGetDotTypeStackInfoRequestData oGetDotTypeStackInfoRequestData = (RegGetDotTypeStackInfoRequestData)oRequestData;       
        xml = DataCache.DataCache.GetCacheData(string.Format("<GetDomainMultiStackPricesWithCurrency><param name=\"n_privateLabelID\" value=\"{0}\"/><param name=\"gdshop_currencyType\" value=\"{1}\"/></GetDomainMultiStackPricesWithCurrency>", oGetDotTypeStackInfoRequestData.PrivateLabelId, oGetDotTypeStackInfoRequestData.CurrencyType));
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        XmlNodeList nodes = xmlDocument.SelectNodes("/data/item");

        foreach (XmlElement node in nodes)
        {
          string promoCode = String.Empty;
          string dotType = String.Empty;
          int price = 0;
          int stackId = 0;

          if (node.HasAttributes)
          {
            if (node.HasAttribute("promo_tracking_code"))
            {
              promoCode = node.GetAttribute("promo_tracking_code");
            }
            if (node.HasAttribute("tld"))
            {
              dotType = node.GetAttribute("tld");
            }
            if (node.HasAttribute("list_price"))
            {
              int.TryParse(node.GetAttribute("list_price"), out price);
            }
            if (node.HasAttribute("stackID"))
            {
              int.TryParse(node.GetAttribute("stackID"), out stackId);
            }
          }

          if (!String.IsNullOrEmpty(promoCode) && !String.IsNullOrEmpty(dotType) && (price != 0) && (stackId != 0))
          {
            if (_dotTypeStackItems.ContainsKey(promoCode))
            {
              _dotTypeStackItems[promoCode].Add(dotType, new DotTypeStackItem(promoCode, dotType, price, stackId));
            }
            else
            {
              Dictionary<string, DotTypeStackItem> items = new Dictionary<string, DotTypeStackItem>();
              items.Add(dotType, new DotTypeStackItem(promoCode, dotType, price, stackId));

              _dotTypeStackItems.Add(promoCode, items);
            }
          }
        }

        oResponseData = new RegGetDotTypeStackInfoResponseData(_dotTypeStackItems, xml);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new RegGetDotTypeStackInfoResponseData(_dotTypeStackItems, xml, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new RegGetDotTypeStackInfoResponseData(_dotTypeStackItems, xml, ex);
      }

      return oResponseData;

    }

    #endregion

  }
}
