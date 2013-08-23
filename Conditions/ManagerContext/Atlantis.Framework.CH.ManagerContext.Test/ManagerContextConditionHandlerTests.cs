using System.Diagnostics;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Atlantis.Framework.CH.SiteContext.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.CH.ManagerContext.dll")]
  public class SiteContextManagerConditionHandlerTests
  {
    private void WriteOutput(string message)
    {
#if (DEBUG)
      Debug.WriteLine(message);
#else
      Console.WriteLine(message);
#endif
    }

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
    public void ManagerInvalidArgumentReturnsFalse()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<IManagerContext, MockManagerContext>();
      container.SetData(MockManagerContextSettings.IsManager, false);

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "isManager(false)";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void CheckReturnsTrueWhenSiteContextIsManagerTrue()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<IManagerContext, MockManagerContext>();
      container.SetData(MockManagerContextSettings.IsManager, true);

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "isManager()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void CheckReturnsFalseWhenSiteContextIsManagerFalse()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<IManagerContext, MockManagerContext>();
      container.SetData(MockManagerContextSettings.IsManager, false);

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "isManager()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void CheckReturnsFalseWhenManagerContextIsNull()
    {
      var container = new MockProviderContainer();

      var expressionParserManager = GetExpressionParserManager(container);

      string expression = "isManager()";
      bool result = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }
  }
}
