﻿using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.TLDDataCache;
using Atlantis.Framework.Providers.TLDDataCache.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.DotTypeCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("dottypecache.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.CH.DotTypeCache.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainContactFields.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DomainContactFields.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.Static.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  public class TldPhaseActiveAnyTests
  {
    private ExpressionParserManager GetExpressionParserManager(IProviderContainer container)
    {
      var expressionParserManager = new ExpressionParserManager(container);
      expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;

      return expressionParserManager;
    }



    private ExpressionParserManager MockContainerExpressionParserManager()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<ITLDDataCacheProvider, TLDDataCacheProvider>();

      var expressionParserManager = GetExpressionParserManager(container);
      return expressionParserManager;
    }

    [TestInitialize]
    public void Initialize()
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
     
    }

    [TestMethod]
    public void InvalidEmptyParameter()
    {
      var expressionParserManager = MockContainerExpressionParserManager();

      const string expression = "tldPhaseActiveAny()";
      var result = expressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void InvalidParameter()
    {
      var expressionParserManager = MockContainerExpressionParserManager();

      const string expression = "tldPhaseActiveAny([fghjfytrtyr])";
      var result = expressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void InvalidMultipleParameters()
    {
      var expressionParserManager = MockContainerExpressionParserManager();

      const string expression = "tldPhaseActiveAny([fhsajfhdsja], fydusaifydsui)";
      var result = expressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void InvalidTLDWithOneValidParameters()
    {
      var expressionParserManager = MockContainerExpressionParserManager();

      const string expression = "tldPhaseActiveAny([fydusaifydsui], GA)";
      var result = expressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidParameter()
    {
      var expressionParserManager = MockContainerExpressionParserManager();

      const string expression = "tldPhaseActiveAny([k.borg], LR)";
      bool result = expressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ValidMultipleParameters()
    {
      var expressionParserManager = MockContainerExpressionParserManager();

      const string expression = "tldPhaseActiveAny([k.borg], GA, LR)";
      var result = expressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ValidCasingParameters()
    {
      var expressionParserManager = MockContainerExpressionParserManager();

      const string expression = "tldPhaseActiveAny([K.bOrG], lR)";
      var result = expressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ValidParametersReturningFalse()
    {
      var expressionParserManager = MockContainerExpressionParserManager();

      const string expression = "tldPhaseActiveAny([MAP], GA)";
      var result = expressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    /*********************************************************************************
     * Commenting these out since we have no punycode/native TLDs to test currently. 
     * Looks as though the expression parser can handles these though.
     *********************************************************************************/

    //[TestMethod]
    //public void PunyCodeParameters()
    //{
    //  var expressionParserManager = MockContainerExpressionParserManager();

    //  const string expression = "tldPhaseActiveAny('xn--mgbcd6b8dd', Sr)";
    //  var result = expressionParserManager.EvaluateExpression(expression);

    //  Assert.IsTrue(result);
    //}

    //[TestMethod]
    //public void NativeCharacterParameters()
    //{
    //  var expressionParserManager = MockContainerExpressionParserManager();

    //  const string expression = "tldPhaseActiveAny('الشبكة', Sr)";
    //  var result = expressionParserManager.EvaluateExpression(expression);

    //  Assert.IsTrue(result);
    //}
  }
}
