using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.RuleEngine.Compiler;
using Atlantis.Framework.RuleEngine.Model;
using Atlantis.Framework.RuleEngine.Results;

namespace Atlantis.Framework.RuleEngine
{
  public sealed class RuleEngine
  {
    private RuleEngineResult _ruleEngineResult;
    private readonly XmlDocument _rules;

    private RuleEngine(Dictionary<string, Dictionary<string, string>> inputModel, XmlDocument rules)
    {
      _rules = rules;
      Evaluate(inputModel);
    }

    private void Evaluate(Dictionary<string, Dictionary<string, string>> inputModels)
    {
      var rom = RuleEngineCompiler.Compile(_rules);

      foreach (var modelKey in inputModels.Keys)
      {
        rom.AddModel(modelKey, inputModels[modelKey]);
      }
      
      rom.Evaluate();

      _ruleEngineResult = new RuleEngineResult(RuleEngineResultStatus.Valid);

      
      _ruleEngineResult.ValidationResults = rom.ModelResults;
      
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
    public static IRuleEngineResult EvaluateRules(Dictionary<string, Dictionary<string, string>> inputModel, XmlDocument rules)
    {
      var ruleEngine = new RuleEngine(inputModel, rules);

      return ruleEngine.GetRuleEngineResult();
    }
  }
}
