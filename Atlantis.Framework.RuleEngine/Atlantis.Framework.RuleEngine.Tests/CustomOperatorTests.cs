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
  public class CustomOperatorTests
  {

    [TestMethod]
    [DeploymentItem("CustomOperatorTests.xml")]
    public void TestShopperValidation()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"CustomOperatorTests.xml");
      rules.Load(directory);

      var model = new Dictionary<string, Dictionary<string, string>>();

      model.Add("Shopper", new Dictionary<string, string>
                            {
                              { "txtCreatePassword", "as32$51df12" },
                              { "passwordMaxLength", "12" },
                              { "passwordMinLength", "8" },
                              { "passwordRegex", @"(?=^\S).*(?=.*[A-Z])(?=.*\d).*(?=\S$)" }
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
          case "txtCreatePassword":
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
