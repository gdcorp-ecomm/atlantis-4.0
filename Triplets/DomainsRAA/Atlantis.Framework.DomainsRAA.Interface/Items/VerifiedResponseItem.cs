namespace Atlantis.Framework.DomainsRAA.Interface.Items
{
  public class VerifiedResponseItem
  {
    public string ItemType { get; private set; }
    public string ItemTypeValue { get; private set; }
    public string ItemValidationGuid { get; private set; }

    public DomainsRAAVerifyCode ItemVerifiedCode { get; private set; }

    public static VerifiedResponseItem Create(string itemType, string itemTypeValue, DomainsRAAVerifyCode itemVerifiedCode, string itemValidationGuid)
    {
      var verifyItem = new VerifiedResponseItem 
      {
        ItemType = itemType, 
        ItemTypeValue = itemTypeValue, 
        ItemValidationGuid = itemValidationGuid, 
        ItemVerifiedCode = itemVerifiedCode};

      return verifyItem;
    }
  }
}
