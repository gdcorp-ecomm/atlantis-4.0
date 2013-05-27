using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Interface
{
  public interface IRoutingRule
  {
    string Type { get; }

    string Condition { get; }

    string Data { get; }
  }
}
