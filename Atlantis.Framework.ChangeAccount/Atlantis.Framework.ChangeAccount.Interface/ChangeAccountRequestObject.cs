namespace Atlantis.Framework.ChangeAccount.Interface
{
  public class ChangeAccountRequestObject
  {
    public string ResourceID { get; set; }
    public string ResourceType { get; set; }
    public string IDType { get; set; }
    public string AccountChangeXML { get; set; }
    public int RenewalPFID { get; set; }
    public int RenewalPeriods { get; set; }
    public string ItemRequestXML { get; set; }

    public ChangeAccountRequestObject(string resourceID, string resourceType,
                        string idType, string accountChangeXML, int renewalPFID,
                        int renewalPeriods, string itemRequestXML)
    {
      ResourceID = resourceID;
      ResourceType = resourceType;
      IDType = idType;
      AccountChangeXML = accountChangeXML;
      RenewalPFID = renewalPFID;
      RenewalPeriods = renewalPeriods;
      ItemRequestXML = itemRequestXML;
    }

  }
}
