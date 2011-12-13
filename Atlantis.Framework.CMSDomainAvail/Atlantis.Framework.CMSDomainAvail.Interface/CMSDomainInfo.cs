using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CMSDomainAvail.Interface
{
  public class CMSDomainInfo
  {
    public string DomainName { get; set; }
    public bool IsValid { get; set; }
    public string AssociatedProduct { get; set; }
  }
}
