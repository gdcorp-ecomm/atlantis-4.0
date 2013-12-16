namespace Atlantis.Framework.DomainsRAA.Interface
{
  public enum DomainsRAAReasonCodes
  {
    None = -1,

    /// <summary>
    /// Verified by FOS Email verification response page
    /// </summary>
    VerifiedByFOSEmail = 1,

    /// <summary>
    /// Phone verified by C3 outreach or incoming phone call    
    /// </summary>
    PhoneVerifiedByC3 = 2,

    /// <summary>
    /// Responded to Annual ICANN email
    /// </summary>
    RespondedAnnualICANNEmail = 3,

    /// <summary>
    /// Used as admin email for completed transfer
    /// </summary>
    UsedAdminEmail = 4,

    /// <summary>
    /// Domain Activation Unverified Registrant
    /// </summary>
    DomainActivationUnverifiedRegistrant = 5,

    /// <summary>
    /// Domain Activation Unverified Shopper
    /// </summary>
    DomainActivationUnverifiedShopper = 6,

    /// <summary>
    /// Contact Update Unverified Registrant
    /// </summary>
    ContactUpdateUnverifiedRegistrant = 7
  }
}
