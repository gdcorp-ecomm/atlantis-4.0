using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.InstantStoreSetup.Interface
{
  public class InstantStoreSetupInfo
  {

    public long BackgroundID { get; set; }
    public string DomainName { get; set; }
    public string OrionAccountUID { get; set; }
    public string SiteDescription { get; set; }
    public string SiteTitle { get; set; }
    public string EmailHash { get; set; }
    public long[] CategoryID { get; set; }
    public string PromoCode { get; set; }

    public InstantStoreSetupInfo()
    {
      DomainName = string.Empty;
      OrionAccountUID = string.Empty;
      SiteDescription = string.Empty;
      SiteTitle = string.Empty;
      EmailHash = string.Empty;
      PromoCode = string.Empty;
    }
  }
}
