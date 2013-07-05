using System;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Personalization;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.Personalization.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.Personalization.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.CH.Personalization.dll")]
  public class TargetMessageAnyConditionHandlerTest
  {
    private const string CONDITION_NAME = "targetMessageTag";

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
          _providerContainer.RegisterProvider<IPersonalizationProvider, MockPersonalizationProvider>();
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

    [TestMethod]
    public void EvaluateCondition1LowerCase()
    {
      string expression = String.Format("{0}({1},{2},{3})", CONDITION_NAME, "EngmtCustServMobileAppMobileHP".ToLower(),"2","Homepage");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
    [TestMethod]
    public void EvaluateCondition2()
    {
      string expression = String.Format("{0}({1},{2},{3})", CONDITION_NAME, "EngmtCustServMobileAppWebHP", "2", "Homepage");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
    [TestMethod]
    public void EvaluateCondition3UpperCase()
    {
      string expression = String.Format("{0}({1},{2},{3})", CONDITION_NAME, "EngmtActNewCustSurveyMobileDLP".ToUpper(), "2", "Homepage");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
    [TestMethod]
    public void EvaluateCondition4()
    {
      string expression = String.Format("{0}({1},{2},{3})", CONDITION_NAME, "EngmtActNewCustSurveyWebDLP", "2", "Homepage");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
    [TestMethod]
    public void EvaluateCondition5()
    {
      string expression = String.Format("{0}({1},{2},{3})", CONDITION_NAME, "TagDoesNotExist", "2", "Homepage");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }
  }
}
