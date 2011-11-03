using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ChangeAccount.Interface
{
  [Serializable]
  public class ChangeAccountRequestData : RequestData
  {
    public List<ChangeAccountRequestObject>  ChangeAccountRequests { get; set; }
    private ChangeAccountRequestData()
      : base("", "", "", "", 0)
    { }

    public ChangeAccountRequestData(string shopperID, string sourceURL, string orderID, 
                          string pathway, int pageCount, string resourceID, string resourceType,
                          string idType, string accountChangeXML, int renewalPFID,
                          int renewalPeriods, string itemRequestXML)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      ResourceID = resourceID;
      ResourceType = resourceType;
      IDType = idType;
      AccountChangeXML = accountChangeXML;
      RenewalPFID = renewalPFID;
      RenewalPeriods = renewalPeriods;
      ItemRequestXML = itemRequestXML;
    }

    public ChangeAccountRequestData(string shopperID, string sourceURL, string orderID,
                         string pathway, int pageCount, List<ChangeAccountRequestObject> changeAccountRequests)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      ChangeAccountRequests = changeAccountRequests;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public string ResourceID { get; set; }
    public string ResourceType { get; set; }
    public string IDType { get; set; }
    public string AccountChangeXML { get; set; }
    public int RenewalPFID { get; set; }
    public int RenewalPeriods { get; set; }
    public string ItemRequestXML { get; set; }

  }
}
