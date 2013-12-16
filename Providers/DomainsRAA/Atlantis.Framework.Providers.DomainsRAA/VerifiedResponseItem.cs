using Atlantis.Framework.Providers.DomainsRAA.Interface;
using Atlantis.Framework.Providers.DomainsRAA.Interface.Items;

namespace Atlantis.Framework.Providers.DomainsRAA
{
  public class VerifiedResponseItem : IVerifiedResponseItem

{
  public string ItemType { get; private set; }
  public string ItemTypeValue { get; private set; }
  public string ItemValidationGuid { get; private set; }

  public DomainsRAAVerifyCode ItemVerifiedCode { get; private set; }

  internal static VerifiedResponseItem Create(string itemType, string itemTypeValue, DomainsRAAVerifyCode itemVerifiedCode, string itemValidationGuid)
  {
    var verifyItem = new VerifiedResponseItem
    {
      ItemType = itemType,
      ItemTypeValue = itemTypeValue,
      ItemValidationGuid = itemValidationGuid,
      ItemVerifiedCode = itemVerifiedCode
    };

    return verifyItem;
  }
}
}
