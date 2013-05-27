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
    private Dictionary<string, ReadOnlyCollection<IRoutingRule>> _readOnlyRulesDict { get; set; }
    private static ReadOnlyCollection<IRoutingRule> NullRoutingRules = new ReadOnlyCollection<IRoutingRule>(new List<IRoutingRule>());

    public RoutingRulesResponseData(string responseData)
      : base(responseData)
    {
      ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);
      List<RoutingRule> rulesList = JsonConvert.DeserializeObject<List<RoutingRule>>(contentVersion.Content);

      Dictionary<string, List<IRoutingRule>> rulesDict = new Dictionary<string, List<IRoutingRule>>();
      foreach (RoutingRule rule in rulesList)
      {
        if (rulesDict.ContainsKey(rule.Type))
        {
          rulesDict[rule.Type].Add(new RoutingRule() { Type = rule.Type, Condition = rule.Condition, Data = rule.Data });
        }
        else
        {
          rulesDict.Add(rule.Type, new List<IRoutingRule>() { new RoutingRule() { Type = rule.Type, Condition = rule.Condition, Data = rule.Data } });
        }
      }

      _readOnlyRulesDict = new Dictionary<string, ReadOnlyCollection<IRoutingRule>>(rulesDict.Count);
      foreach (string key in rulesDict.Keys)
      {
        _readOnlyRulesDict.Add(key, new ReadOnlyCollection<IRoutingRule>(rulesDict[key]));
      }
    }

    public RoutingRulesResponseData(RequestData requestData, Exception exception)
      : base(requestData, exception)
    {
    }

    public bool TryGetValue(string type, out ReadOnlyCollection<IRoutingRule> routingRules)
    {
      bool found = false;
      routingRules = NullRoutingRules;

      if (_readOnlyRulesDict != null)
      {
        if (!_readOnlyRulesDict.TryGetValue(type, out routingRules))
        {
          routingRules = NullRoutingRules;
          found = true;
        }
      }

      return found;
    }

  }
}