using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.Personalization.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Personalization.Impl.dll")]
  public class TargetMessageTests
  {
    private const string _tokenFormat = "[@T[targetmessagename:{0}]@T]";

    [TestInitialize]
    public void InitializeTests()
    {
      TokenManager.RegisterTokenHandler(new TargetMessageTokenHandler());
    }

    private IProviderContainer InitializeContextProviders()
    {
      MockHttpRequest request = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IManagerContext, MockNoManagerContext>();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<IPersonalizationProvider, MockPersonalizationProvider>();
      return result;
    }

    private string TokenSuccess(string xmlTokenData)
    {
      var container = InitializeContextProviders();
      container.Resolve<IShopperContext>().SetNewShopper("12345");

      string token = String.Format(_tokenFormat, xmlTokenData);
      string outputText;
      TokenEvaluationResult result = TokenManager.ReplaceTokens(token, container, out outputText);
      Assert.AreEqual(TokenEvaluationResult.Success, result);
      return outputText;
    }

    [TestMethod]
    public void TargetMessageTokenMatch1()
    {
      string outputText = TokenSuccess("<messagetag name=\"EnGmtACtNewCusTSurvEyMObiLeDLP\" appid=\"2\" interactionpoint=\"Homepage\"></messagetag>");
      Assert.IsTrue(String.Compare(outputText, "EngmtActNewCustSurvey", StringComparison.OrdinalIgnoreCase) == 0);
    }
    [TestMethod]
    public void TargetMessageTokenMatch2()
    {
      string outputText = TokenSuccess("<messagetag name=\"ENGMTActNEWCustSuRveyWEbDLP\" appid=\"2\" interactionpoint=\"Homepage\"></messagetag>");
      Assert.IsTrue(String.Compare(outputText, "EngmtActNewCustSurvey", StringComparison.OrdinalIgnoreCase) == 0);
    }
    [TestMethod]
    public void TargetMessageTokenMatch3()
    {
      string outputText = TokenSuccess("<messagetag name=\"engmtcustservmobileappmobilehp\" appid=\"2\" interactionpoint=\"Homepage\"></messagetag>");
      Assert.IsTrue(String.Compare(outputText, "EngmtCustServMobileApp", StringComparison.OrdinalIgnoreCase) == 0);
    }
    [TestMethod]
    public void TargetMessageTokenMatch4()
    {
      string outputText = TokenSuccess("<messagetag name=\"ENGMTCUSTSERVMOBILEAPPWEBHP\" appid=\"2\" interactionpoint=\"Homepage\"></messagetag>");
      Assert.IsTrue(String.Compare(outputText, "EngmtCustServMobileApp", StringComparison.OrdinalIgnoreCase) == 0);
    }
    [TestMethod]
    public void TargetMessageTokenNoMatch()
    {
      string outputText = TokenSuccess("<messagetag name=\"MessageDoesNotExist\" appid=\"2\" interactionpoint=\"Homepage\"></messagetag>");
      Assert.AreEqual(outputText, String.Empty);
    }
  }
}
