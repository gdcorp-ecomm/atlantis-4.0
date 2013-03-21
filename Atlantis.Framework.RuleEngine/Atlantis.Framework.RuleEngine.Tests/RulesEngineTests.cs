using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.RuleEngine.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RuleEngine.Tests
{
  [TestClass]
  public class RuleEngineTests
  {
    private const string MODEL_ID_EEVALID_TEST = "ExpressionEvaluatorValidTest";

    private const string FACT_ADD = "add";
    private const string FACT_SUB = "sub";
    private const string FACT_MULT = "mult";
    private const string FACT_DIV = "div";
    private const string FACT_GREATER_THAN = "greaterThan";
    private const string FACT_LESS_THAN = "lessThan";
    private const string FACT_GREATER_THAN_OR_EQUALS = "greaterThanOrEquals";
    private const string FACT_LESS_THAN_OR_EQUALS = "lessThanOrEquals";
    private const string FACT_AND = "and";
    private const string FACT_OR = "or";
    private const string FACT_NOT = "not";
    private const string FACT_REGEX = "regex";
    private const string FACT_MIN_LENGTH = "minLength";
    private const string FACT_MAX_LENGTH = "maxLength";

    private const string FACT_ONE_DOUBLE = "oneDouble";
    private const string FACT_ONE_STRING = "oneString";
    private const string FACT_TRUE = "true";
    private const string FACT_FALSE = "false";
    private const string FACT_REGEX_TESTER = "regexTester";
    private const string FACT_REGEX_TO_MATCH = "regexToMatch";
    private const string FACT_MIN_LENGTH_TESTER = "minLengthTester";
    private const string FACT_MIN_LENGTH_VALUE = "minLengthValue";
    private const string FACT_MAX_LENGTH_TESTER = "maxLengthTester";
    private const string FACT_MAX_LENGTH_VALUE = "maxLengthValue";

    Dictionary<string, Dictionary<string, string>> BuildEETestModel(string oneDouble, string oneString,
      string trueBoolean, string falseBoolean, string regexTester, string regexToMatch,
      string minLengthTester, string minLengthValue, string maxLengthTester, string maxLengthValue)
    {
      var model = new Dictionary<string, Dictionary<string, string>>();

      model.Add(MODEL_ID_EEVALID_TEST, new Dictionary<string, string>
                            {
                              { FACT_ADD, string.Empty },
                              { FACT_SUB, string.Empty },
                              { FACT_MULT, string.Empty },
                              { FACT_DIV, string.Empty },
                              { FACT_GREATER_THAN, string.Empty },
                              { FACT_LESS_THAN, string.Empty },
                              { FACT_GREATER_THAN_OR_EQUALS, string.Empty },
                              { FACT_LESS_THAN_OR_EQUALS, string.Empty },
                              { FACT_AND, string.Empty },
                              { FACT_OR, string.Empty },
                              { FACT_NOT, string.Empty },
                              { FACT_REGEX, string.Empty },
                              { FACT_MIN_LENGTH, string.Empty },
                              { FACT_MAX_LENGTH, string.Empty },
                              { FACT_ONE_DOUBLE, oneDouble },
                              { FACT_ONE_STRING, oneString },
                              { FACT_TRUE, trueBoolean },
                              { FACT_FALSE, falseBoolean },
                              { FACT_REGEX_TESTER, regexTester },
                              { FACT_REGEX_TO_MATCH, regexToMatch },
                              { FACT_MIN_LENGTH_TESTER, minLengthTester },
                              { FACT_MIN_LENGTH_VALUE, minLengthValue },
                              { FACT_MAX_LENGTH_TESTER, maxLengthTester },
                              { FACT_MAX_LENGTH_VALUE, maxLengthValue }
                            });

      return model;
    }


    [TestMethod]
    [DeploymentItem("ExpressionEvaluatorValidTest.xml")]
    public void TestValidExpressionEvaluator()
    {
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ExpressionEvaluatorValidTest.xml");
      
      
      XDocument rules = XDocument.Load(directory);
      var model = BuildEETestModel("1", "1", "true", "false",
        "meat", @"[team]*", "football", "7", "snuffleupagus", "14");
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      EvaluateValid(MODEL_ID_EEVALID_TEST, engineResult, modelResults);
      //EvaluateException(engineResult);
    }

    private void EvaluateValid(string modelId, IRuleEngineResult engineResult, IList<IModelResult> modelResults, params string[] failFactKeys)
    {
      Assert.IsTrue(engineResult.Status == RuleEngineResultStatus.Valid);

      var facts = modelResults.FirstOrDefault(m => m.ModelId == modelId);

      Assert.IsNotNull(facts);

      Assert.IsTrue(facts.ContainsInvalids ^ !failFactKeys.Any());

      foreach(var fact in facts.Facts)
      {
        if(failFactKeys.Contains(fact.FactKey))
        {
          Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
          Assert.IsTrue(fact.Messages.Count > 0);
        }
        else
        {
          Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
        }
      }
    }

    private void EvaluateInvalid(IRuleEngineResult engineResult)
    {
      // TODO: Handle internal model validity?
      Assert.IsTrue(engineResult.Status == RuleEngineResultStatus.Invalid);
    }

    private void EvaluateException(IRuleEngineResult engineResult)
    {
      // TODO: HAndle internal model validity?
      Assert.IsTrue(engineResult.Status == RuleEngineResultStatus.Exception);
    }
  }
}
