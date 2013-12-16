namespace Atlantis.Framework.Providers.DomainsRAA.Interface.Items
{
  public class VerifyRequestItem : IVerifyRequestItem
  {
    public string ItemType { get; private set; }
    
    public string ItemTypeValue { get; private set; }

    public static IVerifyRequestItem Create(string itemType, string itemTypeValue)
    {
      var verifyItem = new VerifyRequestItem {ItemType = itemType, ItemTypeValue = itemTypeValue};

      return verifyItem;
    }
  }
}
