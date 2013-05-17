using Atlantis.Framework.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace Atlantis.Framework.CDS.Interface
{
  public class RoutingRulesResponseData : CDSResponseData
  {
    private class RoutingRulesForDeserialization
    {
      [JsonProperty("Type")]
      public string Type { get; set; }

      [JsonProperty("Condition")]
      public string Condition { get; set; }

      [JsonProperty("Data")]
      public string Data { get; set; }
    }
    
    public ReadOnlyCollection<RoutingRule> RoutingRules { get; private set; }

    public RoutingRulesResponseData(string responseData)
      : base(responseData)
    {
      ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);
      List<RoutingRulesForDeserialization> routingRulesForDeserialization = JsonConvert.DeserializeObject<List<RoutingRulesForDeserialization>>(contentVersion.Content);

      List<RoutingRule> routingRulesTemp = new List<RoutingRule>(routingRulesForDeserialization.Count);
      foreach (RoutingRulesForDeserialization rule in routingRulesForDeserialization)
      {
         routingRulesTemp.Add(new RoutingRule(rule.Type, rule.Condition, rule.Data));
      }
      RoutingRules = new ReadOnlyCollection<RoutingRule>(routingRulesTemp);
    }

    public RoutingRulesResponseData(RequestData requestData, Exception exception)
      : base(requestData, exception)
    {
    }
  }
}