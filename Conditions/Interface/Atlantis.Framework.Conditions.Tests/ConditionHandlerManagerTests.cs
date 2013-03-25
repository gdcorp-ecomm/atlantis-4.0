using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.ProviderContainer.Impl;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Render.MarkupParser;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Conditions.Tests
{
  [TestClass]
  public class ConditionHandlerManagerTests
  {
    private const string PRE_PROCESSOR_PREFIX = "##";

    private bool _conditionHandlersRegistered;

    private IProviderContainer _objectProviderContainer;
    private IProviderContainer ObjectProviderContainer
    {
      get
      {
        if (_objectProviderContainer == null)
        {
          _objectProviderContainer = new ObjectProviderContainer();
          _objectProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _objectProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _objectProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
        }

        return _objectProviderContainer;
      }
    }

    private ExpressionParserManager _expressionParserManager;
    private ExpressionParserManager ExpressionParserManager
    {
      get
      {
        if (_expressionParserManager == null)
        {
          _expressionParserManager = new ExpressionParserManager(ObjectProviderContainer);
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

      if (!_conditionHandlersRegistered)
      {
        ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
        _conditionHandlersRegistered = true;
      }
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
    public void EvaludateValidConditionTrue()
    {
      Assert.IsTrue(ConditionHandlerManager.EvaluateCondition("dataCenter", new[] { "AP" }, ObjectProviderContainer));
    }

    [TestMethod]
    public void EvaludateValidConditionFalse()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("dataCenter", new[] { "US" }, ObjectProviderContainer));
    }

    [TestMethod]
    public void EvaludateUnRegisteredCondition()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("doesNotExist", new[] { "value1" }, ObjectProviderContainer));
    }

    [TestMethod]
    public void HtmlFileParseIntegrationTest()
    {
      string finalMarkup = string.Empty;

      string noExpressionsMarkup;
      using (StreamReader htmlFileStream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Atlantis.Framework.Conditions.Tests.Data.home-page-no-conditions.html")))
      {
        noExpressionsMarkup = htmlFileStream.ReadToEnd();
      }

      double startNanoSeconds = DateTime.UtcNow.Ticks;

      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(noExpressionsMarkup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      double endNanoSeconds = DateTime.UtcNow.Ticks;

      double milliSeconds = (endNanoSeconds - startNanoSeconds) / 10000.00;

      WriteOutput(string.Format("No Expressions - Total Parse Time: {0} milliseconds", milliSeconds));


      string withExpressionsMarkup;
      using (StreamReader htmlFileStream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Atlantis.Framework.Conditions.Tests.Data.home-page-with-conditions.html")))
      {
        withExpressionsMarkup = htmlFileStream.ReadToEnd();
      }

      startNanoSeconds = DateTime.UtcNow.Ticks;

      parsedMarkup = MarkupParserManager.ParseAndEvaluate(withExpressionsMarkup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      endNanoSeconds = DateTime.UtcNow.Ticks;

      double milliSecondsWithExpressions = (endNanoSeconds - startNanoSeconds) / 10000.00;

      WriteOutput(string.Format("With Expressions - Total Parse Time: {0} milliseconds", milliSecondsWithExpressions));
      finalMarkup = parsedMarkup;

      WriteOutput(finalMarkup);
    }
  }
}
