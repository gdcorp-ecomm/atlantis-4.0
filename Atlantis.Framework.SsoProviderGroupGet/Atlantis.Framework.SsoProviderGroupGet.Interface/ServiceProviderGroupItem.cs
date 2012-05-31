using System;

namespace Atlantis.Framework.SsoProviderGroupGet.Interface
{
  public class ServiceProviderGroupItem
  {
    public string ServiceProviderGroupName { get; set; }
    public string RedirectLoginUrl { get; set; }
    public string LogoutUrl { get; set; }
    public string RedirectLogoutUrl { get; set; }
    public DateTime CreateDate { get; set; }
    public string ChangedBy { get; set; }
    public string ApprovedBy { get; set; }
    public string ActionDescription { get; set; }
  }
}