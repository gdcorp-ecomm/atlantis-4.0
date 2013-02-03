using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Atlantis.Framework.RuleEngine.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RuleEngine.Tests
{
  [TestClass]
  public class SeRuleEngineTests
  {
    #region Constants
    private const string MODEL_ID_SEVALIDATION = "mdlSe";
    private const string SE_RULE_XML = @"DotSeRule.xml";

    private const string FACT_COUNTRYCODE = "countrycode";
    private const string FACT_VAT = "vat";
    private const string FACT_REGID = "regid";
    #endregion

    #region Supplemental Methods
    Dictionary<string, Dictionary<string, string>> BuildModel(string countrycode, string vat, string regid)
    {
      var model = new Dictionary<string, Dictionary<string, string>>();

      model.Add(MODEL_ID_SEVALIDATION, new Dictionary<string, string>
                            {
                              { FACT_COUNTRYCODE, countrycode },
                              { FACT_VAT, vat },
                              { FACT_REGID, regid }
                            });
      /*
       * <Fact id="FCountryCode" key="countrycode" type="string" modelId="mdlSe" />
       * <Fact id="FVat"         key="vat"         type="string" modelId="mdlSe" />
       * <Fact id="FRegId"       key="regid"       type="string" modelId="mdlSe" />
       */
      return model;
    }
    #endregion

    #region Evaluation (Assert methods)
    private void EvaluateValid(string modelId, IRuleEngineResult engineResult, IEnumerable<IModelResult> modelResults, params string[] failFactKeys)
    {
      Assert.IsTrue(engineResult.Status == RuleEngineResultStatus.Valid);
      var facts = modelResults.FirstOrDefault(m => m.ModelId == modelId);
      Assert.IsNotNull(facts);
      Assert.IsTrue(facts.ContainsInvalids ^ !failFactKeys.Any());

      foreach (var fact in facts.Facts)
      {
        if (failFactKeys.Contains(fact.FactKey))
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
    #endregion

    #region TestMethods
    [TestMethod]
    [DeploymentItem(SE_RULE_XML)]
    public void TestRegIdInValid()
    {
      // Load up the xml rule model
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, SE_RULE_XML);
      var rules = new XmlDocument();
      rules.Load(directory);

      // This is where we set our model fact values
      var model = BuildModel("FR", "12345", string.Empty);
      
      // Execute RuleEngine against the xml model and our rules dictionary
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      // Ensure that the RuleEngine did not have any internal complications, and regid was the only fact evaluated to false
      EvaluateValid(MODEL_ID_SEVALIDATION, engineResult, modelResults, FACT_REGID);
    }

    [TestMethod]
    [DeploymentItem(SE_RULE_XML)]
    public void TestVatInValid()
    {
      // Load up the xml rule model
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, SE_RULE_XML);
      var rules = new XmlDocument();
      rules.Load(directory);

      // This is where we set our model fact values
      var model = BuildModel("FR", string.Empty, "12345");

      // Execute RuleEngine against the xml model and our rules dictionary
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      // Ensure that the RuleEngine did not have any internal complications, and vat was the only fact evaluated to false
      EvaluateValid(MODEL_ID_SEVALIDATION, engineResult, modelResults, FACT_VAT);
    }

    [TestMethod]
    [DeploymentItem(SE_RULE_XML)]
    public void TestValid()
    {
      // Load up the xml rule model
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, SE_RULE_XML);
      var rules = new XmlDocument();
      rules.Load(directory);

      // This is where we set our model fact values
      var model = BuildModel("FR", "1234567890", "12345");

      // Execute RuleEngine against the xml model and our rules dictionary
      var engineResult = RuleEngine.EvaluateRules(model, rules);
      var modelResults = engineResult.ValidationResults;

      // Ensure that the RuleEngine did not have any internal complications, and vat was the only fact evaluated to false
      EvaluateValid(MODEL_ID_SEVALIDATION, engineResult, modelResults);
    }
    #endregion
  }
}