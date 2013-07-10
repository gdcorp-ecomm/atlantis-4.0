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
  public class SpanishTest
  {
    [TestMethod]
    [DeploymentItem("CountrySiteRedirectTests/CountrySiteRedirectRule.xml")]
    public void TestRedirectToES1()
    {
      const string MODEL_ID = "mdlCountryRedirect";
      var rules = new XmlDocument();
      // ReSharper disable AssignNullToNotNullAttribute
      Uri pathUri = new Uri(Path.GetDirectoryName(GetType().Assembly.CodeBase));
      // ReSharper restore AssignNullToNotNullAttribute
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"CountrySiteRedirectRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();
      model.Add(MODEL_ID, new Dictionary<string, string>
                                        {
                                          {"hasCookie", "false"}, 
                                          {"langPref", string.Empty},
                                          {"countryPref", string.Empty}, 
                                          {"isAuth", "false"}, 
                                          {"ipCountry", "US"}, 
                                          {"countrySite", "ES"}
                                        });

      var engineResult = RuleEngine.EvaluateRules(model, rules);

      Assert.IsTrue(engineResult.Status != RuleEngineResultStatus.Exception);

      var modelResults = engineResult.ValidationResults;
      var facts = modelResults.FirstOrDefault(m => m.ModelId == MODEL_ID);

      Assert.IsTrue(facts != null);
      foreach (var fact in facts.Facts)
      {
        switch (fact.FactKey)
        {
          case "countrySite":
            Assert.IsTrue(Convert.ToString(fact.OutputValue) == "ES" || Convert.ToString(fact.OutputValue) == "ES|");
            break;
          default:
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }

    [TestMethod]
    [DeploymentItem("CountrySiteRedirectTests/CountrySiteRedirectRule.xml")]
    public void TestRedirectToES2()
    {
      const string MODEL_ID = "mdlCountryRedirect";
      var rules = new XmlDocument();
      // ReSharper disable AssignNullToNotNullAttribute
      Uri pathUri = new Uri(Path.GetDirectoryName(GetType().Assembly.CodeBase));
      // ReSharper restore AssignNullToNotNullAttribute
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"CountrySiteRedirectRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();
      model.Add(MODEL_ID, new Dictionary<string, string>
                                        {
                                          {"hasCookie", "true"}, 
                                          {"langPref", "ES"},
                                          {"countryPref", "ES"}, 
                                          {"isAuth", "false"}, 
                                          {"ipCountry", "CA"}, 
                                          {"countrySite", "WWW"}
                                        });

      var engineResult = RuleEngine.EvaluateRules(model, rules);

      Assert.IsTrue(engineResult.Status != RuleEngineResultStatus.Exception);

      var modelResults = engineResult.ValidationResults;
      var facts = modelResults.FirstOrDefault(m => m.ModelId == MODEL_ID);

      Assert.IsTrue(facts != null);
      foreach (var fact in facts.Facts)
      {
        switch (fact.FactKey)
        {
          case "countrySite":
            Assert.IsTrue(Convert.ToString(fact.OutputValue) == "ES");
            break;
          default:
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }

    [TestMethod]
    [DeploymentItem("CountrySiteRedirectTests/CountrySiteRedirectRule.xml")]
    public void TestRedirectToES3()
    {
      const string MODEL_ID = "mdlCountryRedirect";
      var rules = new XmlDocument();
      // ReSharper disable AssignNullToNotNullAttribute
      Uri pathUri = new Uri(Path.GetDirectoryName(GetType().Assembly.CodeBase));
      // ReSharper restore AssignNullToNotNullAttribute
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"CountrySiteRedirectRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();

      model.Add(MODEL_ID, new Dictionary<string, string>
                                        {
                                          {"hasCookie", "false"}, 
                                          {"langPref", "ES"},
                                          {"countryPref", ""}, 
                                          {"isAuth", "false"}, 
                                          {"ipCountry", "US"}, 
                                          {"countrySite", "WWW"}
                                        });

      var engineResult = RuleEngine.EvaluateRules(model, rules);

      Assert.IsTrue(engineResult.Status != RuleEngineResultStatus.Exception);

      var modelResults = engineResult.ValidationResults;
      var facts = modelResults.FirstOrDefault(m => m.ModelId == MODEL_ID);

      Assert.IsTrue(facts != null);
      foreach (var fact in facts.Facts)
      {
        switch (fact.FactKey)
        {
          case "countrySite":
            Assert.IsTrue(Convert.ToString(fact.OutputValue) == "ES");
            break;
          default:
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }
  }
}