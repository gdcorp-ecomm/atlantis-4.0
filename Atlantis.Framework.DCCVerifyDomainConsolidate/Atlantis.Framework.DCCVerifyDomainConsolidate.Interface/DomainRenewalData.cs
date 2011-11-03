using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.DCCVerifyDomainConsolidate.Interface
{
  public struct DomainRenewalData
  {
    public int DomainId { get; set; }
    public DateTime SyncExpirationDate { get; set; }
  }
}
