namespace Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems
{
  public class VerifiedResponseItem
  {
    public string ItemType { get; private set; }
    public string ItemTypeValue { get; private set; }
    public string ItemValidationGuid { get; private set; }

    public Framework.DomainsRAA.Interface.DomainsRAAVerifyCode ItemVerifiedCode { get; private set; }

    public static VerifiedResponseItem Create(string itemType, string itemTypeValue, Framework.DomainsRAA.Interface.DomainsRAAVerifyCode itemVerifiedCode, string itemValidationGuid)
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
