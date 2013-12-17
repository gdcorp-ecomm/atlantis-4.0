namespace Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems
{
  public class Item : IItem
  {
    public string ItemType { get; private set; }

    public string ItemTypeValue { get; private set; }

    public static IItem Create(string itemType, string itemTypeValue)
    {
      var verifyItem = new Item { ItemType = itemType, ItemTypeValue = itemTypeValue };

      return verifyItem;
    }
  }
}
