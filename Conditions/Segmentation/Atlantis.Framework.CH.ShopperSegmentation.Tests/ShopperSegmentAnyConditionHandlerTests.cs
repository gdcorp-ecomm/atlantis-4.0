using System;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Segmentation.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.Segmentation.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.CH.Segmentation.dll")]
  public class ShopperSegmentAnyConditionHandlerTests //: ShopperSegmentConditionHandlerTestsBase
  {

    private const string CONDITION_NAME = "shopperSegmentAny";
    private int WebPro = 104;

    [TestInitialize]
    public void Initialize()
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
    }




    private ExpressionParserManager GetExpressionParserManager(int segment)
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<ISegmentationProvider, MockShopperSegmentProvider>();
      container.SetData(MockShopperSegmentProviderSettings.ShopperSegment, segment);
      var expressionParserManager = new ExpressionParserManager(container);
      expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;

      return expressionParserManager;
    }

    [TestMethod]
    public void OneConditionOneParamTrueExpression()
    {
      var expressionParserManager = GetExpressionParserManager(WebPro);
      string expression = String.Format("{0}({1})", CONDITION_NAME, ShopperSegmentations.WebPro);
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void OneConditionOneParamFalseExpression()
    {
      var expressionParserManager = GetExpressionParserManager(WebPro);
      string expression = String.Format("{0}({1})", CONDITION_NAME, ShopperSegmentations.Nacent);
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void OneConditionMultiParamTrueExpression()
    {
      var expressionParserManager = GetExpressionParserManager(WebPro);
      string expression = String.Format("{0}({1},{2},{3},{4},{5})",
        CONDITION_NAME,
        ShopperSegmentations.ActiveBusiness,
        ShopperSegmentations.Domainer,
        ShopperSegmentations.eComm,
        ShopperSegmentations.Nacent,
        ShopperSegmentations.WebPro
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void OneConditionMultiParamFalseExpression()
    {
      var expressionParserManager = GetExpressionParserManager(WebPro);
      string expression = String.Format("{0}({1},{2},{3},{4})",
        CONDITION_NAME,
        ShopperSegmentations.ActiveBusiness,
        ShopperSegmentations.Domainer,
        ShopperSegmentations.eComm,
        ShopperSegmentations.Nacent
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void MultipleConditionOneParamTrueExpression()
    {
      var expressionParserManager = GetExpressionParserManager(WebPro);
      string expression = String.Format("{0}({1}) && !{0}({2})",
        CONDITION_NAME,
        ShopperSegmentations.WebPro,
        ShopperSegmentations.ActiveBusiness
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void MultipleConditionOneParamFalseExpression()
    {
      var expressionParserManager = GetExpressionParserManager(WebPro);
      string expression = String.Format("{0}({1}) && !{0}({2})",
        CONDITION_NAME,
        ShopperSegmentations.ActiveBusiness,
        ShopperSegmentations.WebPro
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void MultipleConditionMultiParamTrueExpression()
    {
      var expressionParserManager = GetExpressionParserManager(WebPro);
      string expression = String.Format("{0}({1},{2},{3}) && !{0}({2},{3},{4})",
        CONDITION_NAME,
        ShopperSegmentations.WebPro,
        ShopperSegmentations.ActiveBusiness,
        ShopperSegmentations.Domainer,
        ShopperSegmentations.eComm
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void MultipleConditionMultiParamFalseExpression()
    {

      var expressionParserManager = GetExpressionParserManager(WebPro);
      string expression = String.Format("{0}({1},{2},{3}) && !{0}({2},{3},{4})",
        CONDITION_NAME,
        ShopperSegmentations.ActiveBusiness,
        ShopperSegmentations.Domainer,
        ShopperSegmentations.eComm,
        ShopperSegmentations.WebPro
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }


    [TestMethod]
    public void SegmentInvalidDefaultNacent()
    {
      var expressionParserManager = GetExpressionParserManager(0);
      string expression = String.Format("{0}({1})",
        CONDITION_NAME,
        ShopperSegmentations.Nacent
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
  }
}

