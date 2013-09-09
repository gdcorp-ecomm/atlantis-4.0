using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Atlantis.Framework.CH.SiteContext.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.CH.SiteContext.dll")]
  public class SiteContextAnyConditionHandlerTests
  {

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
    public void MissingArgumentReturnsFalseAndThrowsException()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void InvalidArgumentReturnsFalse()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(xxxx)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void GdCheckReturnsTrueWhenSiteContextIsGd()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(GD)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void GdCheckReturnsFalseWhenSiteContextIsBr()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "2");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(GD)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void BrCheckReturnsTrueWhenSiteContextIsBr()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "2");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(BR)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void BrCheckReturnsFalseWhenSiteContextIsGd()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(BR)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void WwdCheckReturnsTrueWhenSiteContextIsWwd()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1387");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(WWD)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void WwdCheckReturnsFalseWhenSiteContextIsGd()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(WWD)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void PlCheckReturnsTrueWhenSiteContextIsReseller()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "281241");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(PL)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void PlCheckReturnsFalseWhenSiteContextIsGd()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(PL)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void CheckReturnsTrueWhenOneArgumentMatchesSiteContext()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(PL, wwd, br, gd)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void CheckReturnsFalseWhenNoArgumentMatchesSiteContext()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(PL, wwd, br)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void CheckReturnsTrueWhenFirstArgumentMatchesSiteContext()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.PrivateLabelId, "1");

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "siteContextAny(GD, wwd, br, pl)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }
  }
}
