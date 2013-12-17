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
  public class TargetMessageConditionHandlerTest
  {
    private const string CONDITION_NAME = "targetMessageTagAny";

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
    public void EvaluateCondition1()
    {
      string expression = String.Format("{0}({1},{2})", CONDITION_NAME, "Homepage", "EnGmtACtNewCusTSurvEyMObiLeDLP");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
    [TestMethod]
    public void EvaluateCondition2()
    {
      string expression = String.Format("{0}({1},{2})", CONDITION_NAME, "Homepage", "ENGMTActNEWCustSuRveyWEbDLP");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
    [TestMethod]
    public void EvaluateCondition3()
    {
      string expression = String.Format("{0}({1},{2})", CONDITION_NAME, "Homepage", "engmtcustservmobileappmobilehp");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
    [TestMethod]
    public void EvaluateCondition4()
    {
      string expression = String.Format("{0}({1},{2})", CONDITION_NAME, "Homepage", "ENGMTCUSTSERVMOBILEAPPWEBHP");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
    [TestMethod]
    public void EvaluateCondition5()
    {
      string expression = String.Format("{0}({1},{2})", CONDITION_NAME, "Homepage", "TagDoesNotExist");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsFalse(actual);
    }
    [TestMethod]
    public void MultipleTagNames()
    {
      string expression = String.Format("{0}({1},{2},{3},{4})", CONDITION_NAME, "Homepage", "ENGMTCUSTSERVMOBILEAPPWEBH", "ENGMTCUSTSERVMOBILEAPPWEBHP", "ENGMTCUSTSERVMOBILEAPPWE");
      bool actual = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }
  }
}
