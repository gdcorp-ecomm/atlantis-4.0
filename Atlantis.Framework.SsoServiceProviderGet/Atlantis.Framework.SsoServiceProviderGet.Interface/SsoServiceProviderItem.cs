using System;

namespace Atlantis.Framework.SsoServiceProviderGet.Interface
{
  public class SsoServiceProviderItem
  {
    public string ServiceProviderName { get; set; }
    public string IdentityProviderName { get; set; }
    public string ServiceProviderGroupName { get; set; }
    public string LoginReceive { get; set; }
    public string LoginReceiveType { get; set; }
    public string ServerName { get; set; }
    public bool IsRetired { get; set; }
    public DateTime? RetiredDate { get; set; }
    public DateTime? CreateDate { get; set; }
    public string ChangedBy { get; set; }
    public string ApprovedBy { get; set; }
    public string ActionDescription { get; set; }
  }
}