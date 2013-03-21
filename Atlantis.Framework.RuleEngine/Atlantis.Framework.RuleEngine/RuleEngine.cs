using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.RuleEngine.Compiler;
using Atlantis.Framework.RuleEngine.Results;

namespace Atlantis.Framework.RuleEngine
{
  public sealed class RuleEngine
  {
    private RuleEngineResult _ruleEngineResult;
    private readonly XDocument _rules;

    private RuleEngine(Dictionary<string, Dictionary<string, string>> inputModel, XDocument rules)
    {
      _rules = rules;
      Evaluate(inputModel);
    }

    private void Evaluate(Dictionary<string, Dictionary<string, string>> inputModels)
    {
      try
      {
        var rom = RuleEngineCompiler.Compile(_rules);

        foreach (var modelKey in inputModels.Keys)
        {
          rom.AddModel(modelKey, inputModels[modelKey]);
        }

        rom.Evaluate();

        _ruleEngineResult = new RuleEngineResult(RuleEngineResultStatus.Valid) { ValidationResults = rom.ModelResults };
      }
      catch (Exception ex)
      {
        _ruleEngineResult = new RuleEngineResult(RuleEngineResultStatus.Exception);
        LogSilent(ex, "RuleEngine.Evaluate");
      }
    }

    private void LogSilent(Exception ex, string sourceFunction)
    {
      try
      {
        var siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        var shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
        Engine.Engine.LogAtlantisException(new AtlantisException(sourceFunction, string.Empty, ex.Message, ex.ToString(), siteContext, shopperContext));
      }
      catch{}
    }

    public RuleEngineResult GetRuleEngineResult()
    {
      if (_ruleEngineResult == null)
      {
        _ruleEngineResult = new RuleEngineResult(RuleEngineResultStatus.Exception);
      }

      return _ruleEngineResult;
    }

    /// <summary>
    /// Modile Id Keys and associated value pairs to be used to create Fact results.
    /// </summary>
    /// <param name="inputModel"></param>
    /// <param name="rules"> </param>
    /// <returns></returns>
    [System.Obsolete("For better performance, use EvaluateRules overloaded method having rules parameter type as XDocument")]
    public static IRuleEngineResult EvaluateRules(Dictionary<string, Dictionary<string, string>> inputModel, XmlDocument rules)
    {
      XDocument xDocRules;
      using (var nodeReader = new XmlNodeReader(rules))
      {
        nodeReader.MoveToContent();
        xDocRules = XDocument.Load(nodeReader);
      }

      return EvaluateRules(inputModel, xDocRules);
    }

    /// <summary>
    /// Modile Id Keys and associated value pairs to be used to create Fact results.
    /// </summary>
    /// <param name="inputModel"></param>
    /// <param name="rules"> </param>
    /// <returns></returns>
    public static IRuleEngineResult EvaluateRules(Dictionary<string, Dictionary<string, string>> inputModel, XDocument rules)
    {
      var ruleEngine = new RuleEngine(inputModel, rules);

      return ruleEngine.GetRuleEngineResult();
    }
  }
}
