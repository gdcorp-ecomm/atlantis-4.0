using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.CH.DotTypeCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("dottypecache.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.CH.DotTypeCache.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  public class TldMainPhaseActiveAnyTests
  {
    private Lazy<ExpressionParserManager> _ExpressionParserManager = new Lazy<ExpressionParserManager>(() => GetExpressionParserManager(GetContainer()));
    private Lazy<MockProviderContainer> _ProviderContainer = new Lazy<MockProviderContainer>(() => GetContainer());

    private ExpressionParserManager ExpressionParserManager
    {
      get
      {
        return _ExpressionParserManager.Value;
      }
    }

    private MockProviderContainer ProviderContainer
    {
      get
      {
        return _ProviderContainer.Value;
      }
    }

    [TestInitialize]
    public void Initialize()
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_ConstructorTest()
    {
      var target = new TldMainPhaseActiveAny();
      Assert.IsNotNull(target);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_InvalidPhaseIntegrationTest()
    {
      var target = ExpressionParserManager;
      Assert.IsNotNull(target);
      
      string expression = "tldMainPhaseActiveAny('k.borg', invalid)";
      var actual = target.EvaluateExpression(expression);
      Assert.IsFalse(actual);

      expression = "tldMainPhaseActiveAny('k.borg')";
      actual = target.EvaluateExpression(expression);
      Assert.IsFalse(actual);

    }

    [TestMethod]
    public void TldMainPhaseActiveAny_InvalidPhaseTest()
    {
      var target = new TldMainPhaseActiveAny();
      Assert.IsNotNull(target);

      IList<string> parameters = new List<string>() { "k.borg", "invalid" };
      var actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsFalse(actual);

      parameters = new List<string>() { "k.borg" };
      actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsFalse(actual);

    }

    [TestMethod]
    public void TldMainPhaseActiveAny_InvalidTLDInvalidPhaseIntegrationTest()
    {
      var target = ExpressionParserManager;
      Assert.IsNotNull(target);

      string expression = "tldMainPhaseActiveAny('fhsajfhdsja', invalid)";
      var actual = target.EvaluateExpression(expression);
      Assert.IsFalse(actual);

      expression = "tldMainPhaseActiveAny('fhsajfhdsja')";
      actual = target.EvaluateExpression(expression);
      Assert.IsFalse(actual);

    }

    [TestMethod]
    public void TldMainPhaseActiveAny_InvalidTLDInvalidPhaseTest()
    {
      var target = new TldMainPhaseActiveAny();
      Assert.IsNotNull(target);

      IList<string> parameters = new List<string>() { "fhsajfhdsja", "invalid" };
      var actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsFalse(actual);

      parameters = new List<string>() { "fhsajfhdsja" };
      actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsFalse(actual);

    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsGeneralAvailabilityCheckGeneralAvailabilityIntegrationTest()
    {
      var target = ExpressionParserManager;

      string expression = "tldMainPhaseActiveAny('de', General)";
      bool actual = target.EvaluateExpression(expression);
      Assert.IsTrue(actual);

      expression = "tldMainPhaseActiveAny('de', general)";
      actual = target.EvaluateExpression(expression);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsGeneralAvailabilityCheckGeneralAvailabilityTest()
    {
      var target = new TldMainPhaseActiveAny();
      Assert.IsNotNull(target);

      IList<string> parameters = new List<string>() { "com", "General" };
      var actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsTrue(actual);

      parameters = new List<string>() { "CoM", "general" };
      actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsTrue(actual);
      //parameters = new List<string>() { "la", "general" };
      //actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      //Assert.IsTrue(actual);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsGeneralAvailabilityCheckMultipleIntegrationTest()
    {
      var target = ExpressionParserManager;

      const string expression = "tldMainPhaseActiveAny('com', General, prereg, watchlist)";
      bool result = target.EvaluateExpression(expression);

      Assert.IsTrue(result);

    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsGeneralAvailabilityCheckMultipleTest()
    {
      var target = new TldMainPhaseActiveAny();
      Assert.IsNotNull(target);

      IList<string> parameters = new List<string>() { "de", "General", "PreReg", "WatchList" };
      var actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);

      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsNotGeneralAvailabilityCheckGeneralAvailabilityIntegrationTest()
    {
      var target = ExpressionParserManager;

      const string expression = "tldMainPhaseActiveAny('k.borg', General)";
      bool actual = target.EvaluateExpression(expression);

      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsNotGeneralAvailabilityCheckGeneralAvailabilityTest()
    {
      var target = new TldMainPhaseActiveAny();
      Assert.IsNotNull(target);

      IList<string> parameters = new List<string>() { "k.borg", "general" };
      var actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsFalse(actual);

      parameters = new List<string>() { "k.borg", "General" };
      actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsNotPreRegCheckPreRegIntegrationTest()
    {
      var target = ExpressionParserManager;

      const string expression = "tldMainPhaseActiveAny('map', prereg)";
      bool actual = target.EvaluateExpression(expression);

      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsNotPreRegCheckPreRegTest()
    {
      var target = new TldMainPhaseActiveAny();
      Assert.IsNotNull(target);

      IList<string> parameters = new List<string>() { "map", "PreReg" };
      var actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsFalse(actual);

      parameters = new List<string>() { "map", "prereg" };
      actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsFalse(actual);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsPreRegCheckMultipleIntegrationTest()
    {
      var target = ExpressionParserManager;

      const string expression = "tldMainPhaseActiveAny('map', General, prereg, watchlist)";
      bool result = target.EvaluateExpression(expression);

      Assert.IsTrue(result);

    }

    //[TestMethod]
    //public void TldMainPhaseActiveAny_IsNotWatchlistCheckWatchListIntegrationTest()
    //{
    //  var target = ExpressionParserManager;

    //  const string expression = "tldMainPhaseActiveAny('k.borg', watchlist)";
    //  bool result = target.EvaluateExpression(expression);

    //  Assert.IsFalse(result);
    //}

    //[TestMethod]
    //public void TldMainPhaseActiveAny_IsNotWatchlistCheckWatchlistTest()
    //{
    //  var target = new TldMainPhaseActiveAny();
    //  Assert.IsNotNull(target);

    //  IList<string> parameters = new List<string>() { "k.borg", "WatchList" };
    //  var actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
    //  Assert.IsFalse(actual);
    //}

    [TestMethod]
    public void TldMainPhaseActiveAny_IsPreRegCheckMultipleTest()
    {
      var target = new TldMainPhaseActiveAny();
      Assert.IsNotNull(target);

      IList<string> parameters = new List<string>() { "m.borg", "General", "PreReg", "WatchList" };
      var actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);

      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsPreRegCheckPreRegIntegrationTest()
    {
      var target = new TldMainPhaseActiveAny();

      var expressionParserManager = ExpressionParserManager;

      const string expression = "tldMainPhaseActiveAny('k.borg', PreReg)";
      bool result = expressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsPreRegCheckPreRegTest()
    {
      var target = new TldMainPhaseActiveAny();
      Assert.IsNotNull(target);

      IList<string> parameters = new List<string>() { "K.BORG", "prereg" };
      var actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsTrue(actual);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsWatchlistCheckWatchListIntegrationTest()
    {
      var target = ExpressionParserManager;

      const string expression = "tldMainPhaseActiveAny('map', watchlist)";
      bool result = target.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TldMainPhaseActiveAny_IsWatchlistCheckWatchlistTest()
    {
      var target = new TldMainPhaseActiveAny();
      Assert.IsNotNull(target);

      IList<string> parameters = new List<string>() { "map", "WatchList" };
      var actual = target.EvaluateCondition("tldMainPhaseActiveAny", parameters, this.ProviderContainer);
      Assert.IsTrue(actual);
    }

    private static MockProviderContainer GetContainer()
    {
      var container = new MockProviderContainer();
      container.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      return container;
    }

    private static ExpressionParserManager GetExpressionParserManager(IProviderContainer container)
    {
      var expressionParserManager = new ExpressionParserManager(container);
      expressionParserManager.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;

      return expressionParserManager;
    }

  }
}
