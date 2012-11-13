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
  public class AtlantisRuleEngineTests
  {
    [TestMethod]
    [DeploymentItem("DotSeRule.xml")]
    public void TestSeRegIdInValid()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotSeRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();
      model.Add("mdlSe", new Dictionary<string, string> { { "companycode", "FR" }, {"regid", string.Empty }, {"vat", "12345" } });

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
          //case "companycode":
          //  Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
          //  Assert.IsTrue(!string.IsNullOrEmpty(fact.Message));
          //  break;
          case "regid":
            Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
            Assert.IsTrue(!string.IsNullOrEmpty(fact.Message));
            break;
        }
      }
    }

    [TestMethod]
    [DeploymentItem("DotSeRule.xml")]
    public void TestSeVatInValid()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotSeRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();
      model.Add("mdlSe", new Dictionary<string, string> { { "companycode", "FR" }, { "regid", "12345" }, { "vat", string.Empty } });

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
          case "companycode":
            Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
            Assert.IsTrue(!string.IsNullOrEmpty(fact.Message));
            break;
          //case "regid":
          //  Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
          //  Assert.IsTrue(!string.IsNullOrEmpty(fact.Message));
          //  break;
        }
      }
    }

    //[TestMethod]
    [DeploymentItem("DotUsRule.xml")]
    public void TestUsCompanyType()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotUsRule.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();
      model.Add("mdlUs", new Dictionary<string, string> { { "companytype", "" }, { "islegalreg", "true" } });

      var engineResult = RuleEngine.EvaluateRules(model, rules);

      Assert.IsTrue(engineResult.Status != RuleEngineResultStatus.Exception);

      var modelResults = engineResult.ValidationResults;
      var facts = modelResults.FirstOrDefault(m => m.ModelId == "mdlUs");

      Assert.IsTrue(facts != null);
      Assert.IsTrue(facts.ContainsInvalids);

      foreach (var fact in facts.Facts)
      {
        switch (fact.FactKey)
        {
          case "companytype":
            Assert.IsTrue(fact.Status == ValidationResultStatus.InValid);
            Assert.IsTrue(!string.IsNullOrEmpty(fact.Message));
            break;
          default:
            Assert.IsTrue(fact.Status == ValidationResultStatus.Valid);
            break;
        }
      }
    }

    [TestMethod]
    [DeploymentItem("DotCaRule.xml")]
    public void TestCaLegalTypeRequiresCompany()
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

    //[TestMethod]
    [DeploymentItem("ShopperValidation.xml")]
    public void TestShopperValidation()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"ShopperValidation.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();

    //<Fact id="FFirstName" key="txtFirstName" desc="new value" type="string" modelId="Shopper" />
    //<Fact id="FLastName" key="txtLastName" desc="new value" type="string" modelId="Shopper" />
    //<Fact id="FUserName" key="txtUserName" desc="new value" type="string" modelId="Shopper" />
    //<Fact id="FCreatePassword" key="txtCreatePassword" desc="new value" type="string" modelId="Shopper" />
    //<Fact id="FCreatePassword2" key="txtCreatePassword2" desc="new value" type="string" modelId="Shopper" />
    //<Fact id="FPin" key="txtPin" desc="new value" type="string" modelId="Shopper" />
    //<Fact id="FEmail" key="txtEmail" desc="new value" type="string" modelId="Shopper" />

      model.Add("Shopper", new Dictionary<string, string>
                            {
                              { "txtFirstName", "" },
                              { "txtLastName", "aasdf" },
                              { "txtUserName", "asdf" },
                              { "txtCreatePassword", "asdf" },
                              { "txtCreatePassword2", "asdf" },
                              { "txtPin", "asdf" },
                              { "txtEmail", "asdf" }
                            });

      var engineResult = RuleEngine.EvaluateRules(model, rules);

      Assert.IsTrue(engineResult.Status != RuleEngineResultStatus.Exception);

      var modelResults = engineResult.ValidationResults;
      var facts = modelResults.FirstOrDefault(m => m.ModelId == "Shopper");

      Assert.IsTrue(facts != null);
      Assert.IsTrue(facts.ContainsInvalids);

      //Assert.IsTrue(facts.Facts.ToList().Find(m => m.FactKey == "organization").Status == ValidationResultStatus.InValid);

      foreach (var fact in facts.Facts)
      {

        switch (fact.FactKey)
        {

          case "txtFirstName":

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