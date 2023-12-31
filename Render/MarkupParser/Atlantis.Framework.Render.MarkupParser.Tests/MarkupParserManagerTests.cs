﻿using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Render.MarkupParser.Tests
{
  [TestClass]
  public class MarkupParserManagerTests
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
    public void NullMarkup()
    {
      string markup = null;

      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup == null);
    }

    [TestMethod]
    public void EmptyMarkup()
    {
      string markup = string.Empty;

      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Equals(string.Empty));
    }

    [TestMethod]
    public void SimpleMarkupTestIfCondition()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(AP))
                        Timbo
                        ##endif";

      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Contains("Timbo"));
    }

    [TestMethod]
    public void SimpleMarkupNoConditions()
    {
      string markup = @"Hi my name is Timbo";

      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Equals("Hi my name is Timbo"));
    }

    [TestMethod]
    public void SimpleMarkupTestIfConditionFalse()
    {
      string markup = @"Hi my name is
                        ##if(!dataCenter(AP))
                        Timbo
                        ##endif";

      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsFalse(parsedMarkup.Contains("Timbo"));
    }

    [TestMethod]
    public void SimpleMarkupTestSpaceInParam()
    {
      string markup = @"Hi my name is
                        ##if(!dataCenter(AP, [United States]))
                        Timbo
                        ##endif";

      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsFalse(parsedMarkup.Contains("Timbo"));
    }

    [TestMethod]
    public void WhiteSpaceTrimParamTest()
    {
      string markup = @"Hi my name is
                        ##if(parameterWhitespaceValidator(AP    ,       [United States],   [China]))
                        Timbo
                        ##endif";

      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Contains("Timbo"));
    }

    [TestMethod]
    public void WhiteSpaceBadSyntax()
    {
      string markup = @"Hi my name is
                        ##if(parameterWhitespaceValidator(AP    ,   [United States],   [China]))
                        Timbo
                        ##endif";

      try
      {
        MarkupParserManager.ParseAndEvaluate(markup,
                                             PRE_PROCESSOR_PREFIX,
                                             ExpressionParserManager.EvaluateExpression);
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex is InvalidExpressionException, "Expected \"InvalidExpressionException\" exception but got: \"" + ex.Message + "\"");
      }
    }

    [TestMethod]
    public void BadExpressionSyntax()
    {
      string markup = @"Hi my name is
                        ##if(sdfdssf%^*(AP    ,   [United States],   [China]))
                        Timbo
                        ##endif";

      try
      {
        MarkupParserManager.ParseAndEvaluate(markup,
                                             PRE_PROCESSOR_PREFIX,
                                             ExpressionParserManager.EvaluateExpression);
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex is InvalidExpressionException, "Expected \"InvalidExpressionException\" exception but got: \"" + ex.Message + "\"");
      }
    }

    [TestMethod]
    public void SimpleMarkupTestIfElseCondition()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU))
                        Timbo
                        ##else
                        Billy Bob
                        ##endif";


      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Contains("Billy Bob"));

      Assert.IsFalse(parsedMarkup.Contains("Timbo"));
    }

    [TestMethod]
    public void SimpleMarkupTestIfElseIfCondition()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU))
                        Timbo
                        ##elseif(countrySiteContext(IN))
                        Jimmy John
                        ##else
                        Billy Bob
                        ##endif";


      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Contains("Jimmy John"));

      Assert.IsFalse(parsedMarkup.Contains("Timbo"));
      Assert.IsFalse(parsedMarkup.Contains("Billy Bob"));
    }

    [TestMethod]
    public void SimpleMarkupTestDoubleIfElseIfCondition()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(US))
                        Timbo
                        ##elseif(countrySiteContext(IN))
                        Jimmy John
                        ##elseif(dataCenter(AP))
                        Tommy Gun
                        ##else
                        Billy Bob
                        ##endif";

      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Contains("Jimmy John"));

      Assert.IsFalse(parsedMarkup.Contains("Timbo"));
      Assert.IsFalse(parsedMarkup.Contains("Tommy Gun"));
      Assert.IsFalse(parsedMarkup.Contains("Billy Bob"));
    }

    [TestMethod]
    public void ComplexMarkupTestIfCondition()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,AP) && countrySiteContext(IN))
                        Timbo
                        ##endif";


      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Contains("Timbo"));
    }

    [TestMethod]
    public void ComplexMarkupTestIfConditionFalse()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,US) && countrySiteContext(AU))
                        Timbo
                        ##endif";


      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsFalse(parsedMarkup.Contains("Timbo"));
    }

    [TestMethod]
    public void ComplexMarkupTestIfElseCondition()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,US) && countrySiteContext(AU))
                        Timbo
                        ##else
                        Billy Bob
                        ##endif";


      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Contains("Billy Bob"));

      Assert.IsFalse(parsedMarkup.Contains("Timbo"));
    }

    [TestMethod]
    public void ComplexMarkupTestIfElseIfCondition()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,US) && !countrySiteContext(IN))
                        Timbo
                        ##elseif(dataCenter(AP,EU) && !countrySiteContext(AU))
                        Jimmy John
                        ##else
                        Billy Bob
                        ##endif";


      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Contains("Jimmy John"));

      Assert.IsFalse(parsedMarkup.Contains("Timbo"));
      Assert.IsFalse(parsedMarkup.Contains("Billy Bob"));
    }

    [TestMethod]
    public void ComplexMarkupTestNestedIfElseIfCondition()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,AP) && !countrySiteContext(IN))
                        Timbo
                          ##if(dataCenter(US))
                            Asia Pacific
                          ##elseif(countrySiteContext(IN))
                            Australia
                          ##endif
                        ##elseif(dataCenter(AP,EU) && !countrySiteContext(AU))
                        Jimmy John
                          ##if(dataCenter(AP))
                            Asia Pacific
                          ##elseif(countrySiteContext(AU))
                            Australia
                          ##else
                            Unknown
                          ##endif
                        ##else
                        Billy Bob
                        ##endif";


      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      WriteOutput(parsedMarkup);

      Assert.IsTrue(parsedMarkup.Contains("Jimmy John"));
      Assert.IsTrue(parsedMarkup.Contains("Asia Pacific"));

      Assert.IsFalse(parsedMarkup.Contains("Timbo"));
      Assert.IsFalse(parsedMarkup.Contains("Billy Bob"));
    }

    [TestMethod]
    public void BadSyntaxMissingInvalidIfPreProcessor()
    {
      string markup = @"Hi my name is
                        ##ifbad(dataCenter(EU,US) && !countrySiteContext(IN))
                        Timbo
                        ##elseif(dataCenter(AP,EU) && !countrySiteContext(AU))
                        Jimmy John
                        ##else
                        Billy Bob
                        ##endif";


      try
      {
        MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);
      }
      catch (InvalidExpressionException invalidExpressionException)
      {
        WriteOutput(invalidExpressionException.Message);
        Assert.IsTrue(invalidExpressionException.Message.Contains("Invalid pre-preocessor"));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void BadSyntaxMissingInvalidIfElsePreProcessor()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,US) && !countrySiteContext(IN))
                        Timbo
                        ##elseifbad(dataCenter(AP,EU) && !countrySiteContext(AU))
                        Jimmy John
                        ##else
                        Billy Bob
                        ##endif";


      try
      {
        MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);
      }
      catch (InvalidExpressionException invalidExpressionException)
      {
        WriteOutput(invalidExpressionException.Message);
        Assert.IsTrue(invalidExpressionException.Message.Contains("Invalid pre-preocessor"));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void BadSyntaxMissingInvalidElsePreProcessor()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,US) && !countrySiteContext(IN))
                        Timbo
                        ##elseif(dataCenter(AP,EU) && !countrySiteContext(AU))
                        Jimmy John
                        ##elsebad
                        Billy Bob
                        ##endif";


      try
      {
        MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);
      }
      catch (InvalidExpressionException invalidExpressionException)
      {
        WriteOutput(invalidExpressionException.Message);
        Assert.IsTrue(invalidExpressionException.Message.Contains("Invalid pre-preocessor"));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void BadSyntaxMissingEndIf()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,US) && !countrySiteContext(IN))
                        Timbo
                        Jimmy John
                        Billy Bob";


      try
      {
        MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);
      }
      catch (InvalidExpressionException invalidExpressionException)
      {
        WriteOutput(invalidExpressionException.Message);
        Assert.IsTrue(invalidExpressionException.Message.Contains("\"##endif\" expected."));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void BadSyntaxMissingStartingIf()
    {
      string markup = @"Hi my name is
                        ##elseif(dataCenter(AU))
                        Timbo
                        ##endif";


      try
      {
        MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);
      }
      catch (InvalidExpressionException invalidExpressionException)
      {
        WriteOutput(invalidExpressionException.Message);
        Assert.IsTrue(invalidExpressionException.Message.Contains("Missing starting ##if condition."));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void BadSyntaxMissingEndIfComplex()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,US) && !countrySiteContext(IN))
                        Timbo
                        ##elseif(dataCenter(AP,EU) && !countrySiteContext(AU))
                        Jimmy John
                        ##else
                        Billy Bob";


      try
      {
        MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);
      }
      catch (InvalidExpressionException invalidExpressionException)
      {
        WriteOutput(invalidExpressionException.Message);
        Assert.IsTrue(invalidExpressionException.Message.Contains("\"##endif\" expected."));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void BadSyntaxNestedMissingEndIf()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,US) && !countrySiteContext(IN))
                        Timbo
                          ##if(countrySiteContext(US))
                          United States
                        ##elseif(dataCenter(AP,EU) && !countrySiteContext(AU))
                        Jimmy John
                        ##else
                        Billy Bob
                        ##endif";


      try
      {
        MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);
      }
      catch (InvalidExpressionException invalidExpressionException)
      {
        WriteOutput(invalidExpressionException.Message);
        Assert.IsTrue(invalidExpressionException.Message.Contains("\"##endif\" expected."));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void BadSyntaxNestedMissingEndIfVariationTwo()
    {
      string markup = @"Hi my name is
                        ##if(dataCenter(EU,US) && !countrySiteContext(IN))
                        Timbo
                          ##if(countrySiteContext(US))
                          United States
                          ##else
                          Other
                        ##elseif(dataCenter(AP,EU) && !countrySiteContext(AU))
                        Jimmy John
                        ##else
                        Billy Bob
                        ##endif";


      try
      {
        MarkupParserManager.ParseAndEvaluate(markup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);
      }
      catch (InvalidExpressionException invalidExpressionException)
      {
        WriteOutput(invalidExpressionException.Message);
        Assert.IsTrue(invalidExpressionException.Message.Contains("\"##endif\" expected."));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    public void HtmlFileParseIntegrationTest()
    {
      string finalMarkup = string.Empty;

      string noExpressionsMarkup;
      using (StreamReader htmlFileStream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Atlantis.Framework.Render.MarkupParser.Tests.Data.home-page-no-conditions.html")))
      {
        noExpressionsMarkup = htmlFileStream.ReadToEnd();
      }

      double startMilliseconds = DateTime.UtcNow.Ticks;

      string parsedMarkup = MarkupParserManager.ParseAndEvaluate(noExpressionsMarkup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      double endMilliseconds = DateTime.UtcNow.Ticks;

      double milliSeconds = (endMilliseconds - startMilliseconds) / 10000.00;

      WriteOutput(string.Format("No Expressions - Total Parse Time: {0} milliseconds", milliSeconds));


      string withExpressionsMarkup;
      using (StreamReader htmlFileStream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Atlantis.Framework.Render.MarkupParser.Tests.Data.home-page-with-conditions.html")))
      {
        withExpressionsMarkup = htmlFileStream.ReadToEnd();
      }

      startMilliseconds = DateTime.UtcNow.Ticks;

      parsedMarkup = MarkupParserManager.ParseAndEvaluate(withExpressionsMarkup, PRE_PROCESSOR_PREFIX, ExpressionParserManager.EvaluateExpression);

      endMilliseconds = DateTime.UtcNow.Ticks;

      double milliSecondsWithExpressions = (endMilliseconds - startMilliseconds) / 10000.00;

      WriteOutput(string.Format("With Expressions - Total Parse Time: {0} milliseconds", milliSecondsWithExpressions));
      finalMarkup = parsedMarkup;

      WriteOutput(finalMarkup);
    }
  }
}
