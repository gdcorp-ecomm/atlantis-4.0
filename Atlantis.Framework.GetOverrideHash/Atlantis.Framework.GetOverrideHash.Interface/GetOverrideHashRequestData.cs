using System;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.GetOverrideHash.Interface
{
  public class GetOverrideHashRequestData : RequestData
  {
    public GetOverrideHashRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      UnifiedPFID = 0;
      PrivateLabelID = 0;
      OverrideCurrentPrice = 0;
      OverrideCurrentCost = 0;
      GetCostHash = false;
    }

    public GetOverrideHashRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int unifiedPfid, int privateLabelId, int overrideListPrice, int overrideCurrentPrice)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      UnifiedPFID = unifiedPfid;
      PrivateLabelID = privateLabelId;
      OverrideListPrice = overrideListPrice;
      OverrideCurrentPrice = overrideCurrentPrice;
      OverrideCurrentCost = 0;
      GetCostHash = false;
    }

    public GetOverrideHashRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int unifiedPfid, int privateLabelId, int overrideListPrice, int overrideCurrentPrice, int overrideCurrentCost)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      UnifiedPFID = unifiedPfid;
      PrivateLabelID = privateLabelId;
      OverrideListPrice = overrideListPrice;
      OverrideCurrentPrice = overrideCurrentPrice;
      OverrideCurrentCost = overrideCurrentCost;
      GetCostHash = true;
    }

    public GetOverrideHashRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int unifiedPfid, int privateLabelId, int priceTypeId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      UnifiedPFID = unifiedPfid;
      PrivateLabelID = privateLabelId;
      OverridePriceTypeId = priceTypeId;
      GetCostHash = false;
      GetPriceTypeHash = true;
    }

    public int UnifiedPFID { get; set; }
    public int PrivateLabelID { get; set; }
    public int OverrideListPrice { get; set; }
    public int OverrideCurrentPrice { get; set; }
    public int OverrideCurrentCost { get; set; }
    public int OverridePriceTypeId { get; set; }
    public bool GetCostHash { get; private set; }
    public bool GetPriceTypeHash { get; private set; }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("GetOverrideHash is not a cacheable request");
    }

    public override string ToXML()
    {
      XElement element = new XElement("GetOverrideHash");
      element.Add(new XAttribute("unifiedproductid", UnifiedPFID.ToString()));
      element.Add(new XAttribute("privatelabelid", PrivateLabelID.ToString()));
      element.Add(new XAttribute("listprice", OverrideListPrice.ToString()));
      element.Add(new XAttribute("currentprice", OverrideCurrentPrice.ToString()));
      element.Add(new XAttribute("currentcost", OverrideCurrentCost.ToString()));
      element.Add(new XAttribute("costhash", GetCostHash.ToString()));
      element.Add(new XAttribute("pricehash", GetPriceTypeHash.ToString()));
      return element.ToString();
    }

    #endregion
  }
}
