using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RuleEngine.Tests
{
  [TestClass]
  public class AtlantisRuleEngineTests
  {
    [TestMethod]
    [DeploymentItem("DotCaRule.xml")]
    [DeploymentItem("Atlantis.Framework.RuleEngine.dll")]
    public void TestLegalTypeRequiresCompany()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var  assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotCaRule.xml");
      rules.Load(directory);
      var rom = Compiler.Compiler.Compile(rules);

      var model = new XmlDocument();
      model.LoadXml("<result><org><value/><isvalid/></org><legaltype><value/></legaltype></result>");
      model.SelectSingleNode("result/org/value").InnerText = "Some Company";
      model.SelectSingleNode("result/org/isvalid").InnerText = true.ToString();
      model.SelectSingleNode("result/legaltype/value").InnerText = "ASS";

      rom.AddModel("mdlOrg", model);
      
      rom.Evaluate();

      Debug.WriteLine(model.OuterXml);
      Assert.IsTrue(model.SelectSingleNode("result/org/isvalid").InnerText == "True");

    }

    [TestMethod]
    [DeploymentItem("DotCaRule.xml")]
    [DeploymentItem("Atlantis.Framework.RuleEngine.dll")]
    public void TestLegalTypeNotRequiresCompany()
    {
      var rules = new XmlDocument();
      Uri pathUri = new Uri(Path.GetDirectoryName(this.GetType().Assembly.CodeBase));
      var assemblyPath = pathUri.LocalPath;
      string directory = Path.Combine(assemblyPath, @"DotCaRule.xml");
      rules.Load(directory);
      var rom = Compiler.Compiler.Compile(rules);

      var model = new XmlDocument();
      model.LoadXml("<result><org><value/><isvalid/></org><legaltype><value/></legaltype></result>");
      model.SelectSingleNode("result/org/value").InnerText = "";
      model.SelectSingleNode("result/org/isvalid").InnerText = true.ToString();
      model.SelectSingleNode("result/legaltype/value").InnerText = "CCT";

      rom.AddModel("mdlOrg", model);

      rom.Evaluate();

      Debug.WriteLine(model.OuterXml);
      Assert.IsFalse(model.SelectSingleNode("result/org/isvalid").InnerText != "True");

    }
  }
}