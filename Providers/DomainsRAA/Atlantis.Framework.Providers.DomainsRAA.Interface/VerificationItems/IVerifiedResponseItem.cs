namespace Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems
{
  public interface IVerifiedResponseItem
  {
    string ItemType { get; }
    string ItemTypeValue { get;}
    string ItemValidationGuid { get; }
    DomainsRAAVerifyCode ItemVerifiedCode { get; }
  }
}
