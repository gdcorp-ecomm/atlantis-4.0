using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using System.Reflection;
using Atlantis.Framework.Providers.Segmentation.Interface;

namespace Atlantis.Framework.CH.ShopperSegment.Tests
{

  public class ShopperSegmentConditionHandlerTestsBase
  {
    public ShopperSegmentConditionHandlerTestsBase()
    {

    }

    private IProviderContainer _providerContainer;
    protected IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          MockProviderContainer mockProviderContainer = new MockProviderContainer();

          _providerContainer = mockProviderContainer;
          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
          _providerContainer.RegisterProvider<ISegmentationProvider, MockShopperSegmentProvider>();
        }

        return _providerContainer;
      }
    }

    private ExpressionParserManager _expressionParserManager;
    protected ExpressionParserManager ExpressionParserManager
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
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
    }
  }

  [TestClass]
  public class ShopperSegmentAllConditionHandlerTests : ShopperSegmentConditionHandlerTestsBase
  {
    private const string CONDITION_NAME = "ShopperSegmentAll";


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
      string expression = String.Format("{0}(6,6,6,6)", CONDITION_NAME);
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
      string expression = String.Format("{0}(6,6,6,6) && !{0}(1,2,3)", CONDITION_NAME);
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
