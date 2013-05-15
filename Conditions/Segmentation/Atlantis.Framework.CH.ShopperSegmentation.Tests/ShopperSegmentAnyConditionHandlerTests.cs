using System;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.ShopperSegment.Tests
{
  [TestClass]
  public class ShopperSegmentAnyConditionHandlerTests : ShopperSegmentConditionHandlerTestsBase
  {

    private const string CONDITION_NAME = "ShopperSegmentAny";

    [TestMethod]
    public void OneConditionOneParamTrueExpression()
    {
      string expression = String.Format("{0}(6)", CONDITION_NAME);
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void OneConditionOneParamFalseExpression()
    {
      string expression = String.Format("{0}(1)", CONDITION_NAME);
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void OneConditionMultiParamTrueExpression()
    {
      string expression = String.Format("{0}(1,2,3,4,5,6)", CONDITION_NAME);
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void OneConditionMultiParamFalseExpression()
    {
      string expression = String.Format("{0}(1,2,3,4,5)", CONDITION_NAME);
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void MultipleConditionOneParamTrueExpression()
    {
      string expression = String.Format("{0}(6) && !ShopperSegmentAny(2)", CONDITION_NAME);
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void MultipleConditionOneParamFalseExpression()
    {
      string expression = String.Format("{0}(7) && !{0}(6)", CONDITION_NAME);
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void MultipleConditionMultiParamTrueExpression()
    {
      string expression = String.Format("{0}(6,7,8,9) && !{0}(1,2,3)", CONDITION_NAME);
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void MultipleConditionMultiParamFalseExpression()
    {
      string expression = String.Format("{0}(7,8,9) && !{0}(1,2,3,4,5)", CONDITION_NAME);
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }
  }
}

