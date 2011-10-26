using System.Xml;

namespace Atlantis.Framework.PurchaseEmail.Interface.Emails
{
  internal class MerchantInfo
  {
    private string _shopId;
    XmlElement _merchantItemElement;

    public MerchantInfo(string shopId)
    {
      _shopId = shopId;

      string merchantInfoXml = DataCache.DataCache.GetCacheData("<GetMktPlcShopInfoByID><param name=\"n_resource_id\" value=\"" + _shopId + "\"/></GetMktPlcShopInfoByID>");
      XmlDocument merchantDoc = new XmlDocument();
      merchantDoc.LoadXml(merchantInfoXml);

      _merchantItemElement = merchantDoc.SelectSingleNode("/item") as XmlElement;
    }

    public string ShopId
    {
      get { return _shopId; }
    }

    public string MarketPlaceName
    {
      get { return _merchantItemElement.GetAttribute("marketPlaceName"); }
    }

    public string MarketPlaceDescription
    {
      get { return _merchantItemElement.GetAttribute("marketPlaceDescription"); }
    }

    public string SupportEmailAddress
    {
      get { return _merchantItemElement.GetAttribute("supportEmailAddress"); }
    }

    public string SupportPhone
    {
      get { return _merchantItemElement.GetAttribute("supportPhone"); }
    }

  }
}
