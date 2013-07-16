using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
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

    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
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
    public void EvaluateValidConditionTrue()
    {
      Assert.IsTrue(ConditionHandlerManager.EvaluateCondition("dataCenter", new[] { "AP" }, ProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionFalse()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("dataCenter", new[] { "US" }, ProviderContainer));
    }

    [TestMethod]
    public void EvaluateUnRegisteredCondition()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("doesNotExist", new[] { "value1" }, ProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionNullParam()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("dataCenter", new [] { "US", null }, ProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionNullParamList()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("dataCenter", null, ProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionEmptyParamList()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("dataCenter", new List<string>(0), ProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionParamWithSpace()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("dataCenter", new[] { "United States" }, ProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionTwoParamWithSpace()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("dataCenter", new[] { "IN", "United States" }, ProviderContainer));
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
