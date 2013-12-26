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
  public class RequestIsInternalConditionHandlerTests
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
    public void RequestIsInternalTrue()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.IsRequestInternal, true);

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "requestIsInternal()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void RequestIsInternalFalse()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.SetMockSetting(MockSiteContextSettings.IsRequestInternal, false);

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "requestIsInternal()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }
  }
}
