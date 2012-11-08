﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Atlantis.Framework.RuleEngine.Compiler;
using Atlantis.Framework.RuleEngine.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RuleEngine.Tests
{
  [TestClass]
  public class AtlantisRuleEngineTests
  {
    [TestMethod]
    [DeploymentItem("DotCaRule.xml")]
    public void TestLegalTypeRequiresCompany()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotCaRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();

      model.Add("mdlOrg", new Dictionary<string, string> { { "organization", "" }, { "legaltype", "CCO" } });

      var engineResult = RuleEngine.EvaluateRules(model, rules);

      var modelResults = engineResult.ValidationResults;
      var facts = modelResults.FirstOrDefault(m => m.ModelId == "mdlOrg");

      Assert.IsTrue(facts != null);
      Assert.IsTrue(facts.ContainsInvalids);

      Assert.IsTrue(facts.Facts.ToList().Find(m => m.FactKey == "organization").Status == ValidationResultStatus.InValid);
      
    }

    //[TestMethod]
    //[DeploymentItem("DotCaRule.xml")]
    //public void TestLegalTypeIsValid()
    //{
    //  var rules = new XmlDocument();
    //  Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
    //  var assemblyPath = pathUri.LocalPath;
    //  string directory = Path.Combine(assemblyPath, @"DotCaRule.xml");
    //  rules.Load(directory);

    //  var model = new Dictionary<string, Dictionary<string, string>>();

    //  model.Add("mdlOrg", new Dictionary<string, string> { { "organization", "Some Value" }, { "legaltype", "CCO" } });

    //  var engineResult = RuleEngine.EvaluateRules(model, rules);

    //  var validationResults = engineResult.ValidationResults;
    //  var result = validationResults["mdlOrg"];

    //  Assert.IsTrue(result.Status == ValidationResultStatus.Valid);

    //}

    //[TestMethod]
    //[DeploymentItem("DotCaRule.xml")]
    //public void TestLegalTypeRequiresCompany()
    //{
    //  var rules = new XmlDocument();
    //  Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
    //  var  assemblyPath = pathUri.LocalPath;
    //  string directory = Path.Combine(assemblyPath, @"DotCaRule.xml");
    //  rules.Load(directory);
    //  var rom = RuleEngineCompiler.Compile(rules);

    //  var model = new XmlDocument();
    //  model.LoadXml("<result><org><value/><isvalid/></org><legaltype><value/></legaltype></result>");
    //  model.SelectSingleNode("result/org/value").InnerText = "Some Company";
    //  model.SelectSingleNode("result/org/isvalid").InnerText = true.ToString();
    //  model.SelectSingleNode("result/legaltype/value").InnerText = "ASS";

    //  rom.AddModel("mdlOrg", model);
      
    //  rom.Evaluate();

    //  Debug.WriteLine(model.OuterXml);
    //  Assert.IsTrue(model.SelectSingleNode("result/org/isvalid").InnerText == "True");

    //}

    //[TestMethod]
    //[DeploymentItem("DotCaRule.xml")]
    //[DeploymentItem("Atlantis.Framework.RuleEngine.dll")]
    //public void TestLegalTypeNotRequiresCompany()
    //{
    //  var rules = new XmlDocument();
    //  Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
    //  var assemblyPath = pathUri.LocalPath;
    //  string directory = Path.Combine(assemblyPath, @"DotCaRule.xml");
    //  rules.Load(directory);
    //  var rom = Compiler.RuleEngineCompiler.Compile(rules);

    //  var model = new XmlDocument();
    //  model.LoadXml("<result><org><value/><isvalid/></org><legaltype><value/></legaltype></result>");
    //  model.SelectSingleNode("result/org/value").InnerText = "";
    //  model.SelectSingleNode("result/org/isvalid").InnerText = true.ToString();
    //  model.SelectSingleNode("result/legaltype/value").InnerText = "CCT";

    //  rom.AddModel("mdlOrg", model);

    //  rom.Evaluate();

    //  Debug.WriteLine(model.OuterXml);
    //  Assert.IsFalse(model.SelectSingleNode("result/org/isvalid").InnerText != "True");

    //}
  }
}