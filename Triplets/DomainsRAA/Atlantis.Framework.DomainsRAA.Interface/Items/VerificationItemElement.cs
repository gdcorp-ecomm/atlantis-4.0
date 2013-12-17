namespace Atlantis.Framework.DomainsRAA.Interface.Items
{
  public class VerificationItemElement
  {
    public string ShopperId { get; private set; }
    public string RequestedIp { get; private set; }

    public DomainsRAAReasonCodes ReasonCode { get; private set; }

    public VerificationItemsElement VerificationItems { get; private set; }

    public static VerificationItemElement Create(string shopperId, string requestedIp, VerificationItemsElement verificationItemsElement, DomainsRAAReasonCodes reasonCode)
    {
      var verifyItems = new VerificationItemElement
      {
        ShopperId = shopperId,
        RequestedIp = requestedIp,
        VerificationItems = verificationItemsElement,
        ReasonCode = reasonCode,
      };

      return verifyItems;
    }
  }
}
