using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiPurchase.Interface
{
  public class BonsaiPurchaseRequestData : RequestData
  {
    public BonsaiPurchaseRequestData(string shopperID, string sourceURL, string orderID,
                                     string pathway, int pageCount, string resourceID, string resourceType,
                                     string idType, string accountPurchaseChangeXML, int renewalPFID,
                                     int renewalPeriods, string itemRequestXML)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      ResourceID = resourceID;
      ResourceType = resourceType;
      IDType = idType;
      AccountPurchaseChangeXML = accountPurchaseChangeXML;
      RenewalPFID = renewalPFID;
      RenewalPeriods = renewalPeriods;
      ItemRequestXML = itemRequestXML;
      RequestTimeout = TimeSpan.FromSeconds(5d);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public string ResourceID { get; set; }
    public string ResourceType { get; set; }
    public string IDType { get; set; }
    public string AccountPurchaseChangeXML { get; set; }
    public int RenewalPFID { get; set; }
    public int RenewalPeriods { get; set; }
    public string ItemRequestXML { get; set; }

  }
}
