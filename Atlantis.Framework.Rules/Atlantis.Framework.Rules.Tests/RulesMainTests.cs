using System;
using Atlantis.Framework.Rules.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Rules.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Rules.Impl.dll")]
  public class RulesMainTests
  {
    private RulesMainResponseData GetRulesData(string ruleName)
    {
      var request = new RulesMainRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, ruleName);
      var response = (RulesMainResponseData)DataCache.DataCache.GetProcessRequest(request, 672);
      return response;
    }

    [TestMethod]
    public void GetRulesDataRCaMain()
    {
      var caMain = GetRulesData("RCaMain");
      Assert.IsTrue(caMain != null && caMain.Rules != null && !string.IsNullOrEmpty(caMain.ToXML()));
    }

    [TestMethod]
    public void RequestDataToXml()
    {
      const string ruleName = "TestRuleName";
      var requestData = new RulesMainRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, ruleName);
      Assert.IsTrue(!string.IsNullOrEmpty(requestData.ToXML()));
    }

    [TestMethod]
    public void ResponseDataToXml()
    {
      RulesMainResponseData failData = null;
      try
      {
        failData = GetRulesData("");
      }
      catch (Exception)
      {
      }
      finally
      {
        Assert.IsTrue(failData == null);
      }
    }
  }
}
