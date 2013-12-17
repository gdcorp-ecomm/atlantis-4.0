namespace Atlantis.Framework.DomainsRAA.Interface.Items
{
  public class ItemElement 
  {
    public string ItemType { get; private set; }

    public string ItemTypeValue { get; private set; }

    public static ItemElement Create(string itemType, string itemTypeValue)
    {
      var verifyItem = new ItemElement {ItemType = itemType, ItemTypeValue = itemTypeValue};

      return verifyItem;
    }
  }
}
