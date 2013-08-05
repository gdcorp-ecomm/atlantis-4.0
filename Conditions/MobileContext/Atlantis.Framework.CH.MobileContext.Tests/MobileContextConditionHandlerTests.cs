using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.MobileContext;
using Atlantis.Framework.Providers.MobileContext.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.MobileContext.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.CH.MobileContext.dll")]
  public class MobileContextConditionHandlerTests
  {
    private const string IPHONE_USER_AGENT = "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5";
    private const string CHROME_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.95 Safari/537.36";
    private const string FIREFOX_USER_AGENT = "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.0.18) Gecko/2010020220 Firefox/3.0.18";
    private const string ANDROID_USER_AGENT = "Mozilla/5.0 (Linux; U; Android 2.1-update1; en-us; Nexus One Build/ERE27) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17";

    private ExpressionParserManager GetExpressionParserManager(IProviderContainer container)
    {
      var expressionParserManager = new ExpressionParserManager(container);
      expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;

      return expressionParserManager;
    }

    [TestInitialize]
    public void Initialize()
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
    }

    [TestMethod]
    public void MobileApplicationTypeIsAppHasArgument()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=1");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileApplicationTypeIsApp(xxx)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MobileApplicationTypeIsAppTrue()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=1");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileApplicationTypeIsApp()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MobileApplicationTypeIsAppFalse()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileApplicationTypeIsApp()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void MobileApplicationTypeAnyMissingArgument()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=1");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileApplicationTypeAny()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void MobileApplicationTypeAnyInvalidArgument()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=1");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileApplicationTypeAny(xxx)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void MobileApplicationTypeAnySingleValid()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=1");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileApplicationTypeAny(iphone)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MobileApplicationTypeAnySingleValidOthersFalse()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=1");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileApplicationTypeAny(iphone, ipad, android)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MobileApplicationTypeAnyAllFalse()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(CHROME_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileApplicationTypeAny(iphone, ipad, android)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void MobileApplicationTypeAnyNone()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(CHROME_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileApplicationTypeAny(none)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MobileDeviceTypeAnyMissingArgument()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileDeviceTypeAny()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void MobileDeviceTypeAnyInvalidArgument()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileDeviceTypeAny(xxx)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void MobileDeviceTypeAnySingleValid()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(ANDROID_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileDeviceTypeAny(android)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MobileDeviceTypeAnySingleValidOthersFalse()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=1");
      mockHttpRequest.MockUserAgent(ANDROID_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileDeviceTypeAny(iphone, ipad, android)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MobileDeviceTypeAnyAllFalse()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(CHROME_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileDeviceTypeAny(iphone, ipad)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void MobileViewTypeAnyMissingArgument()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileViewTypeAny()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void MobileViewTypeAnyInvalidArgument()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileViewTypeAny(xxx)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void MobileViewTypeAnySingleValid()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(FIREFOX_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileViewTypeAny(firefox)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MobileViewTypeAnySingleValidOthersFalse()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=1");
      mockHttpRequest.MockUserAgent(FIREFOX_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileViewTypeAny(webkit, opera, firefox)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MobileViewTypeAnyAllFalse()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(FIREFOX_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      var container = new MockProviderContainer();
      container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      var expressionParserManager = GetExpressionParserManager(container);

      const string expression = "mobileViewTypeAny(apple, android, iphone, ipad)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }
  }
}