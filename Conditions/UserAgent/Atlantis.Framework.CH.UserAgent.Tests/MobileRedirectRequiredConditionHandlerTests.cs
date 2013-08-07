using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.MobileRedirect;
using Atlantis.Framework.Providers.MobileRedirect.Interface;
using Atlantis.Framework.Providers.UserAgentDetection;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.UserAgent.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.UserAgentEx.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.CH.UserAgent.dll")]
  public class MobileRedirectRequiredConditionHandlerTests
  {
    private const string IPHONE_USER_AGENT = "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5";
    private const string CHROME_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.95 Safari/537.36";
    private const string GOOGLE_BOT_USER_AGENT = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";

    private IProviderContainer ProviderContainer
    {
      get
      {
        IProviderContainer providerContainer = new MockProviderContainer();
        providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
        providerContainer.RegisterProvider<IUserAgentDetectionProvider, UserAgentDetectionProvider>();
        providerContainer.RegisterProvider<IMobileRedirectProvider, MobileRedirectProvider>();

        return providerContainer;
      }
    }

    private ExpressionParserManager _expressionParserManager;
    private ExpressionParserManager ExpressionParserManager
    {
      get
      {
        if (_expressionParserManager == null)
        {
          _expressionParserManager = new ExpressionParserManager(ProviderContainer);
          _expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;
        }

        return _expressionParserManager;
      }
    }

    [TestInitialize]
    public void Initialize()
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers();
    }

    [TestMethod]
    public void IphoneRedirectTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);

      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      string expression = "mobileRedirectRequired()";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ChromeUserAgentTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(CHROME_USER_AGENT);

      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      string expression = "mobileRedirectRequired()";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void GoogleBotUserAgentTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(GOOGLE_BOT_USER_AGENT);

      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      string expression = "mobileRedirectRequired()";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void NullUserAgentTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(null);

      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      string expression = "mobileRedirectRequired()";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void EmptyUserAgentTest()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(string.Empty);

      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      string expression = "mobileRedirectRequired()";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void NoHttpContextTest()
    {
      string expression = "mobileRedirectRequired()";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void ProviderNotRegistered()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/home");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);

      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      // Empty provider container
      ExpressionParserManager expressionParserManager = new ExpressionParserManager(new MockProviderContainer());
      expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;

      string expression = "mobileRedirectRequired()";
      bool result = expressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }
  }
}
