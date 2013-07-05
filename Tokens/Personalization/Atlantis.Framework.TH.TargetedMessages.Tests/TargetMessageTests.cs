using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Personalization;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Tokens.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.TH.TargetedMessages.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Personalization.Impl.dll")]
  public class TargetMessageTests
  {
    private const string _tokenFormat = "[@T[targetmessageid:{0}]@T]";

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
      string outputText = TokenSuccess("<messagetag name=\"EngmtActNewCustSurveyMobileDLP\" appid=\"2\" interactionpoint=\"Homepage\"></messagetag>");
      Assert.IsTrue(String.Compare(outputText, "7f8426c1-e9de-491f-9bcb-19fd8351d22a", StringComparison.OrdinalIgnoreCase) == 0);
    }
    [TestMethod]
    public void TargetMessageTokenMatch2()
    {
      string outputText = TokenSuccess("<messagetag name=\"EngmtActNewCustSurveyWebDLP\" appid=\"2\" interactionpoint=\"Homepage\"></messagetag>");
      Assert.IsTrue(String.Compare(outputText, "7f8426c1-e9de-491f-9bcb-19fd8351d22a", StringComparison.OrdinalIgnoreCase) == 0);
    }
    [TestMethod]
    public void TargetMessageTokenMatch3()
    {
      string outputText = TokenSuccess("<messagetag name=\"EngmtCustServMobileAppMobileHP\" appid=\"2\" interactionpoint=\"Homepage\"></messagetag>");
      Assert.IsTrue(String.Compare(outputText, "0e554af3-9851-4a99-90f0-1d11987ea553", StringComparison.OrdinalIgnoreCase) == 0);
    }
    [TestMethod]
    public void TargetMessageTokenMatch4()
    {
      string outputText = TokenSuccess("<messagetag name=\"EngmtCustServMobileAppWebHP\" appid=\"2\" interactionpoint=\"Homepage\"></messagetag>");
      Assert.IsTrue(String.Compare(outputText, "0e554af3-9851-4a99-90f0-1d11987ea553", StringComparison.OrdinalIgnoreCase) == 0);
    }
    [TestMethod]
    public void TargetMessageTokenNoMatch()
    {
      string outputText = TokenSuccess("<messagetag name=\"MessageDoesNotExist\" appid=\"2\" interactionpoint=\"Homepage\"></messagetag>");
      Assert.AreEqual(outputText, String.Empty);
    }
  }
}
