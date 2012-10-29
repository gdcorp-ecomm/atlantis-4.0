using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Entities.Common.Interfaces
{
  public interface ICDSDictionary
  {
    Dictionary<string, string> CDSDictionary { get; set; }
  }
}
