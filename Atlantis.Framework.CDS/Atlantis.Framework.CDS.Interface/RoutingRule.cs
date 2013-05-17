using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Interface
{
  public class RoutingRule
  {
    public string Type { get; private set; }

    public string Condition { get; private set; }

    public string Data { get; private set; }

    public RoutingRule(string type, string condition, string data)
    {
      Type = type;
      Condition = condition;
      Data = data;
    }
  }
}
