using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.MyaCheckDomainRenewalStatus.Interface
{
  public struct DomainRenewalData
  {
    public int DomainId { get; set; }
    public int RenewalLength { get; set; }
  }
}
