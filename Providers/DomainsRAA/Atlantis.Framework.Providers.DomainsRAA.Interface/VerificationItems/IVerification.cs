namespace Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems
{
  public interface IVerification
  {
    ReasonCodes ReasonCode { get; }
    VerificationItems VerifyItems { get; }
  }
}
