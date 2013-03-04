using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Conditions.Tests.ConditionHandlers;
using Atlantis.Framework.ExpressionParser;
using Atlantis.Framework.Providers.ProviderContainer.Impl;
using Atlantis.Framework.Render.MarkupParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Conditions.Tests
{
  [TestClass]
  public class ConditionHandlerManagerTests
  {
    private const string PRE_PROCESSOR_PREFIX = "##";

    private bool _conditionHandlersRegistered;

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
      ExpressionParserManager.EvaluateFunctionHandler += ConditionHandlerManager.EvaluateCondition;

      if (!_conditionHandlersRegistered)
      {
        ConditionHandlerManager.RegisterConditionHandler(new DataCenterCondtionHandler());
        ConditionHandlerManager.RegisterConditionHandler(new CountrySiteContextConditionHandler());
        ConditionHandlerManager.RegisterConditionHandler(new AprimoMessageIdConditionHandler());

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
    public void HtmlFileParseTest()
    {
      string finalMarkup = string.Empty;

      string noExpressionsMarkup;
      using (StreamReader htmlFileStream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Atlantis.Framework.Conditions.Tests.Data.home-page-no-conditions.html")))
      {
        noExpressionsMarkup = htmlFileStream.ReadToEnd();
      }

      double startMilliseconds = DateTime.UtcNow.Ticks;

      StringBuilder parsedMarkup = MarkupParserManager.ParseAndEvaluate(noExpressionsMarkup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      double endMilliseconds = DateTime.UtcNow.Ticks;

      double milliSeconds = (endMilliseconds - startMilliseconds) / 10000.00;

      WriteOutput(string.Format("No Expressions - Total Parse Time: {0} milliseconds", milliSeconds));


      string withExpressionsMarkup;
      using (StreamReader htmlFileStream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Atlantis.Framework.Conditions.Tests.Data.home-page-with-conditions.html")))
      {
        withExpressionsMarkup = htmlFileStream.ReadToEnd();
      }

      startMilliseconds = DateTime.UtcNow.Ticks;

      parsedMarkup = MarkupParserManager.ParseAndEvaluate(withExpressionsMarkup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      endMilliseconds = DateTime.UtcNow.Ticks;

      double milliSecondsWithExpressions = (endMilliseconds - startMilliseconds) / 10000.00;

      WriteOutput(string.Format("With Expressions - Total Parse Time: {0} milliseconds", milliSecondsWithExpressions));
      finalMarkup = parsedMarkup.ToString();

      WriteOutput(finalMarkup);
    }
  }
}
