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
    [TestMethod]
    [DeploymentItem("DotSeRule.xml")]
    public void TestRegIdInValid()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotSeRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();
      model.Add("mdlSe", new Dictionary<string, string> { { "countrycode", "FR" }, {"regid", string.Empty }, {"vat", "12345" } });

      var engineResult = RuleEngine.EvaluateRules(model, rules);

     Assert.IsTrue(engineResult.Status != RuleEngineResultStatus.Exception);

     var modelResults = engineResult.ValidationResults;

      var facts = modelResults.FirstOrDefault(m => m.ModelId == "mdlSe");

      Assert.IsTrue(facts != null);
      Assert.IsTrue(facts.ContainsInvalids);

      foreach (var fact in facts.Facts)
      {
        switch (fact.FactKey)
        {
          case "countrycode":
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
          case "regid":
            Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
            Assert.IsTrue(!string.IsNullOrEmpty(fact.Message));
            break;
          case "vat":
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }

    [TestMethod]
    [DeploymentItem("DotSeRule.xml")]
    public void TestVatInValid()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotSeRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();
      model.Add("mdlSe", new Dictionary<string, string> { { "countrycode", "FR" }, { "regid", "12345" }, { "vat", string.Empty } });

      var engineResult = RuleEngine.EvaluateRules(model, rules);

      Assert.IsTrue(engineResult.Status != RuleEngineResultStatus.Exception);

      var modelResults = engineResult.ValidationResults;

      var facts = modelResults.FirstOrDefault(m => m.ModelId == "mdlSe");

      Assert.IsTrue(facts != null);
      Assert.IsTrue(facts.ContainsInvalids);

      foreach (var fact in facts.Facts)
      {
        switch (fact.FactKey)
        {
          case "countrycode":
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
          case "regid":
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
          case "vat":
            Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
            Assert.IsTrue(!string.IsNullOrEmpty(fact.Message));
            break;
        }
      }
    }

    [TestMethod]
    [DeploymentItem("DotSeRule.xml")]
    public void TestValid()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotSeRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();
      model.Add("mdlSe", new Dictionary<string, string> { { "countrycode", "FR" }, { "regid", "12345" }, { "vat", "1234567890" } });

      var engineResult = RuleEngine.EvaluateRules(model, rules);

      Assert.IsTrue(engineResult.Status != RuleEngineResultStatus.Exception);

      var modelResults = engineResult.ValidationResults;

      var facts = modelResults.FirstOrDefault(m => m.ModelId == "mdlSe");

      Assert.IsTrue(facts != null);
      Assert.IsTrue(!facts.ContainsInvalids);

      foreach (var fact in facts.Facts)
      {
        switch (fact.FactKey)
        {
          case "countrycode":
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
          case "regid":
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
          case "vat":
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }

  }
}