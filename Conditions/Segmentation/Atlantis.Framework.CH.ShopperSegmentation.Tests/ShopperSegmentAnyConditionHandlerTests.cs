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
    private string segmentId = "webpro";

    string Nacent = "Nacent";
    string ActiveBusiness = "ActiveBusiness";
    string eComm = "EComm";
    string WebPro = "WebPro";
    string Domainer = "Domainer";

    [TestInitialize]
    public void Initialize()
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
    }




    private ExpressionParserManager GetExpressionParserManager(string segment)
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<IShopperSegmentationProvider, MockShopperSegmentProvider>();
      container.SetData(MockShopperSegmentProviderSettings.ShopperSegment, segment);
      var expressionParserManager = new ExpressionParserManager(container);
      expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;

      return expressionParserManager;
    }

    [TestMethod]
    public void OneConditionOneParamTrueExpression()
    {
      var expressionParserManager = GetExpressionParserManager(segmentId);
      string expression = String.Format("{0}({1})", CONDITION_NAME, WebPro);
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void OneConditionOneParamFalseExpression()
    {
      var expressionParserManager = GetExpressionParserManager(segmentId);
      string expression = String.Format("{0}({1})", CONDITION_NAME, Nacent);
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void OneConditionMultiParamTrueExpression()
    {
      var expressionParserManager = GetExpressionParserManager(segmentId);
      string expression = String.Format("{0}({1},{2},{3},{4},{5})",
        CONDITION_NAME,
        ActiveBusiness,
        Domainer,
        eComm,
        Nacent,
        WebPro
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void OneConditionMultiParamFalseExpression()
    {
      var expressionParserManager = GetExpressionParserManager(segmentId);
      string expression = String.Format("{0}({1},{2},{3},{4})",
        CONDITION_NAME,
        ActiveBusiness,
        Domainer,
        eComm,
        Nacent
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void MultipleConditionOneParamTrueExpression()
    {
      var expressionParserManager = GetExpressionParserManager(segmentId);
      string expression = String.Format("{0}({1}) && !{0}({2})",
        CONDITION_NAME,
        WebPro,
        ActiveBusiness
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void MultipleConditionOneParamFalseExpression()
    {
      var expressionParserManager = GetExpressionParserManager(segmentId);
      string expression = String.Format("{0}({1}) && !{0}({2})",
        CONDITION_NAME,
        ActiveBusiness,
        WebPro
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void MultipleConditionMultiParamTrueExpression()
    {
      var expressionParserManager = GetExpressionParserManager(segmentId);
      string expression = String.Format("{0}({1},{2},{3}) && !{0}({2},{3},{4})",
        CONDITION_NAME,
        WebPro,
        ActiveBusiness,
        Domainer,
        eComm
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void MultipleConditionMultiParamFalseExpression()
    {

      var expressionParserManager = GetExpressionParserManager(segmentId);
      string expression = String.Format("{0}({1},{2},{3}) && !{0}({2},{3},{4})",
        CONDITION_NAME,
        ActiveBusiness,
        Domainer,
        eComm,
        WebPro
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }


    [TestMethod]
    public void SegmentInvalidDefaultNacent()
    {
      var expressionParserManager = GetExpressionParserManager(null);
      string expression = String.Format("{0}({1})",
        CONDITION_NAME,
        Nacent
        );
      bool actual = expressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
  }
}

