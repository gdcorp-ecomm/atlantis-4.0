using System;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockLocalization;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.Localization.Tests
{
  [TestClass]
  public class CountrySiteAnyConditionHandlerTests
  {
    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          MockProviderContainer mockProviderContainer = new MockProviderContainer();
          mockProviderContainer.SetMockSetting(MockLocalizationProviderSettings.CountrySite, "www");

          _providerContainer = mockProviderContainer;
          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
          _providerContainer.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();

          
        }

        return _providerContainer;
      }
    }

    private ExpressionParserManager _expressionParserManager;
    private ExpressionParserManager ExpressionParserManager
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

    private void WriteOutput(string message)
    {
#if (DEBUG)
      Debug.WriteLine(message);
#else
      Console.WriteLine(message);
#endif
    }

    [TestMethod]
    public void OneConditionExpression()
    {
      string expression = "countrySiteAny(us)";
      bool result = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MultipleConditionExpression()
    {
      string expression = "countrySiteAny(us) && !countrySiteAny(in)";
      bool result = ExpressionParserManager.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }
  }
}
