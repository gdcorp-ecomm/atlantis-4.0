namespace Atlantis.Framework.DomainsRAA.Interface.Items
{
  public class VerifyRequestItem 
  {
    public string ItemType { get; private set; }


    public string ItemTypeValue { get; private set; }

    public static VerifyRequestItem Create(string itemType, string itemTypeValue)
    {
      var verifyItem = new VerifyRequestItem {ItemType = itemType, ItemTypeValue = itemTypeValue};

      return verifyItem;
    }
  }
}
