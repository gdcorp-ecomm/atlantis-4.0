namespace Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems
{
  public class Verification : IVerification
  {
    public ReasonCodes ReasonCode { get; private set; }

    public VerificationItems VerifyItems { get; private set; }
    
    public static Verification Create(ReasonCodes reasonCode, VerificationItems verificationItems)
    {
      var verifyItem = new Verification
      {
        VerifyItems = verificationItems,
        ReasonCode = reasonCode,
      };

      return verifyItem;
    }
  }
}
