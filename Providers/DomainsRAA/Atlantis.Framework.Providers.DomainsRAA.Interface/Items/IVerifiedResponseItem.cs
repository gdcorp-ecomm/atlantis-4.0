namespace Atlantis.Framework.Providers.DomainsRAA.Interface.Items
{
  public interface IVerifiedResponseItem
  {
    string ItemType { get; }
    string ItemTypeValue { get;}
    string ItemValidationGuid { get; }
    DomainsRAAVerifyCode ItemVerifiedCode { get; }
  }
}
