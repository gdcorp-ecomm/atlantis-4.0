using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CDS.Interface
{
  internal class RoutingRule : IRoutingRule
  {
    [JsonProperty("Type")]
    public string Type { get; set; }

    [JsonProperty("Condition")]
    public string Condition { get; set; }

    [JsonProperty("Data")]
    public string Data { get; set; }
  }
}
