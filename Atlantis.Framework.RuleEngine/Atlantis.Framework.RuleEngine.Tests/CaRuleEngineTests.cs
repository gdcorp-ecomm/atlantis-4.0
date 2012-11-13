﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Atlantis.Framework.RuleEngine.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RuleEngine.Tests
{
  [TestClass]
  public class CaRuleEngineTests
  {
    [TestMethod]
    [DeploymentItem("DotCaRule.xml")]
    public void TestCaCompany()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotCaRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();
      model.Add("mdlOrg", new Dictionary<string, string> {{"organization", string.Empty}, {"legaltype", "CCO"}});

      var engineResult = RuleEngine.EvaluateRules(model, rules);

      Assert.IsTrue(engineResult.Status != RuleEngineResultStatus.Exception);

      var modelResults = engineResult.ValidationResults;
      var facts = modelResults.FirstOrDefault(m => m.ModelId == "mdlOrg");

      Assert.IsTrue(facts != null);
      Assert.IsTrue(facts.ContainsInvalids);

      foreach (var fact in facts.Facts)
      {
        switch (fact.FactKey)
        {
          case "organization":
            Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
            Assert.IsTrue(!string.IsNullOrEmpty(fact.Message));
            break;
          default:
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }
  }
}