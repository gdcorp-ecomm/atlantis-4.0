using System;

namespace Atlantis.Framework.SsoIdentityProviderGet.Interface
{
  public class SsoIdentityProviderItem
  {
    public string IdentityProviderName { get; set; }
    public string LoginUrl { get; set; }
    public string LogoutUrl { get; set; }
    public string PublicKey { get; set; }
    public string CertificateName { get; set; }
    public DateTime CreateDate { get; set; }
    public string ChangedBy { get; set; }
    public string ApprovedBy { get; set; }
    public string ActionDescription { get; set; }
  }
}
