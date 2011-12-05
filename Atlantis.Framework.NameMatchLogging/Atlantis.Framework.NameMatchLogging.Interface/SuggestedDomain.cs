using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.NameMatchLogging.Interface
{
  public class SuggestedDomain
  {
    public string Order { get; set; }
    public string Tld { get; set; }
    public string Sld { get; set; }
    public string DomainName
    {
      get
      {
        return Sld + "." + Tld;
      }
    }   
  }
}
