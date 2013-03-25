using Atlantis.Framework.Providers.ProviderContainer.Impl;
using Atlantis.Framework.Render.ExpressionParser.Tests.EvaluateFunctionHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Render.ExpressionParser.Tests
{
  [TestClass]
  public class ExpressionParserManagerTests
  {
    private ExpressionParserManager _expressionParserManager;
    private ExpressionParserManager ExpressionParserManager
    {
      get
      {
        if (_expressionParserManager == null)
        {
          _expressionParserManager = new ExpressionParserManager(new ObjectProviderContainer());
        }

        return _expressionParserManager;
      }
    }

    [TestInitialize]
    public void Initialize()
    {
      ExpressionParserManager.EvaluateExpressionHandler += EvaluateFunctionHandlerFactory.EvaluateFunctionHandler;
    }

    [TestMethod]
    public void TestSimpleExpressionTrue()
    {
      string expression = "(dataCenter(AP))";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TestSimpleExpressionFalse()
    {
      string expression = "(dataCenter(EU))";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestComplexExpression()
    {
      string expression = "(dataCenter(EU,AP) && (!dataCenter(AP) || countrySiteContext(IN)))";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TestSimpleExpressionCaching()
    {
      string expression = "(dataCenter(AP))";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result); // dataCenter will be EU

      expression = "(dataCenter(EU))";
      result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result); // dataCenter will be AP

      expression = "(dataCenter(US))";
      result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result); // dataCenter will be US

      expression = "(dataCenter(AP))";
      result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result); // dataCenter will be EU again
    }
  }
}
