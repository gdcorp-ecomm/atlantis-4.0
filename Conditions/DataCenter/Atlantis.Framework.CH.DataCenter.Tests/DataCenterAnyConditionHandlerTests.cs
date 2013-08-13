using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DataCenter.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.DataCenter.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.CH.DataCenter.dll")]
  public class DataCenterAnyConditionHandlerTests
  {
    private IProviderContainer ProviderContainer
    {
      get
      {
        MockProviderContainer mockProviderContainer = new MockProviderContainer();

        mockProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
        mockProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
        mockProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
        mockProviderContainer.RegisterProvider<IGeoProvider, MockGeoProvider>();
        mockProviderContainer.RegisterProvider<IDataCenterProvider, MockDataCenterProvider>();

        return mockProviderContainer;
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
      ConditionHandlerManager.AutoRegisterConditionHandlers();
    }

    [TestMethod]
    public void InValidEmptyParameters()
    {
      string expression = "dataCenterAny()";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void InValidOneParameter()
    {
      string expression = "dataCenterAny(asdfasdfdsdf)";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void InValidMultipleParameters()
    {
      string expression = "dataCenterAny(asdfasdfdsdf, sjdfjksdjkl)";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidOneParameter()
    {
      string expression = "dataCenterAny(us)";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ValidMultipleParameters()
    {
      string expression = "dataCenterAny(us, eu)";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ValidWithOneInvalidParameter()
    {
      string expression = "dataCenterAny(us, asdfasfdasfsf)";
      bool result = ExpressionParserManager.EvaluateExpression(expression);

      Assert.IsTrue(result);
    }
  }
}
