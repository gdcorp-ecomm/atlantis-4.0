using Atlantis.Framework.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atlantis.Framework.CDS.Interface
{
  public class RoutingRulesResponseData : CDSResponseData
  {
    public ContentId Id { get; private set; }
    private static readonly ReadOnlyCollection<IRoutingRule> _nullRoutingRules = new ReadOnlyCollection<IRoutingRule>(new List<IRoutingRule>(0));
    private static readonly IDictionary<string, ReadOnlyCollection<IRoutingRule>> _emptyRulesDictionary = new Dictionary<string, ReadOnlyCollection<IRoutingRule>>(0);

    private readonly IDictionary<string, ReadOnlyCollection<IRoutingRule>> _readOnlyRulesDictionary = _emptyRulesDictionary;
  
    public RoutingRulesResponseData(string responseData) : base(responseData)
    {
      ContentVersion contentVersion = JsonConvert.DeserializeObject<ContentVersion>(responseData);
      Id = contentVersion._id;

      List<RoutingRule> rulesList = JsonConvert.DeserializeObject<List<RoutingRule>>(contentVersion.Content);

      if (rulesList != null)
      {
        Dictionary<string, List<IRoutingRule>> rulesDict = new Dictionary<string, List<IRoutingRule>>(rulesList.Count);

        foreach (RoutingRule rule in rulesList)
        {
          if (rulesDict.ContainsKey(rule.Type))
          {
            rulesDict[rule.Type].Add(new RoutingRule { Type = rule.Type, Condition = rule.Condition, Data = rule.Data });
          }
          else
          {
            rulesDict.Add(rule.Type, new List<IRoutingRule> { new RoutingRule { Type = rule.Type, Condition = rule.Condition, Data = rule.Data } });
          }
        }

        _readOnlyRulesDictionary = new Dictionary<string, ReadOnlyCollection<IRoutingRule>>(rulesDict.Count);

        foreach (string key in rulesDict.Keys)
        {
          _readOnlyRulesDictionary.Add(key, new ReadOnlyCollection<IRoutingRule>(rulesDict[key]));
        }
      }
    }

    public RoutingRulesResponseData(RequestData requestData, Exception exception) : base(requestData, exception)
    {
    }

    public bool TryGetValue(string type, out ReadOnlyCollection<IRoutingRule> routingRules)
    {
      bool found = false;

      if (!_readOnlyRulesDictionary.TryGetValue(type, out routingRules))
      {
        routingRules = _nullRoutingRules;
        found = true;
      }

      return found;
    }
  }
}