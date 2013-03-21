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
  
  public class EuRuleEngineTests
  {
    [TestMethod]
    [DeploymentItem("DotEuRule.xml")]
    public void TestUsCompanyTypeRequired()
    {
      
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotEuRule.xml");
      XDocument rules = XDocument.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();
      model.Add("mdlEu", new Dictionary<string, string> { { "countrycode", "US" } });

      var engineResult = RuleEngine.EvaluateRules(model, rules);

      Assert.IsTrue(engineResult.Status != RuleEngineResultStatus.Exception);

      var modelResults = engineResult.ValidationResults;
      var facts = modelResults.FirstOrDefault(m => m.ModelId == "mdlEu");

      Assert.IsTrue(facts != null);
      Assert.IsTrue(facts.ContainsInvalids);

      foreach (var fact in facts.Facts)
      {
        switch (fact.FactKey)
        {
          case "countrycode":
            Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
            Assert.IsTrue(fact.Messages.Count > 0);
            break;
          default:
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }
  }
}