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
  public class AustraliaSiteTest
  {
    [TestMethod]
    [DeploymentItem("CountrySiteRedirectTests/CountrySiteRedirectRule.xml")]
    public void TestRedirectToAU1()
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
                                          {"ipCountry", "AU"}, 
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
            Assert.IsTrue(Convert.ToString(fact.OutputValue) == "AU" || Convert.ToString(fact.OutputValue) == "AU|");
            break;
          default:
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }

    [TestMethod]
    [DeploymentItem("CountrySiteRedirectTests/CountrySiteRedirectRule.xml")]
    public void TestRedirectToAU2()
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
                                          {"langPref", ""},
                                          {"countryPref", ""}, 
                                          {"isAuth", "false"}, 
                                          {"ipCountry", "US"}, 
                                          {"countrySite", "AU"}
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
            Assert.IsTrue(Convert.ToString(fact.OutputValue) == "AU");
            break;
          default:
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }

    [TestMethod]
    [DeploymentItem("CountrySiteRedirectTests/CountrySiteRedirectRule.xml")]
    public void TestRedirectToAU3()
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
                                          {"langPref", "EN"},
                                          {"countryPref", "AU"}, 
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
            Assert.IsTrue(Convert.ToString(fact.OutputValue) == "AU");
            break;
          default:
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }

    [TestMethod]
    [DeploymentItem("CountrySiteRedirectTests/CountrySiteRedirectRule.xml")]
    public void TestRedirectToAU5()
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
                                          {"hasCookie", "False"}, 
                                          {"langPref", string.Empty},
                                          {"countryPref", string.Empty}, 
                                          {"isAuth", "false"}, 
                                          {"ipCountry", "AU"}, 
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
            Assert.IsTrue(Convert.ToString(fact.OutputValue) == "AU");
            break;
          default:
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }
  }
}