using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using System.Net;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo;
using Atlantis.Framework.Conditions.Interface;
using System.Reflection;
using Atlantis.Framework.Render.ExpressionParser;

namespace Atlantis.Framework.CH.Geo.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Geo.Impl.dll")]
  [DeploymentItem("GeoIP.dat")]
  [DeploymentItem("GeoIPLocation.dat")]
  [DeploymentItem("Atlantis.Framework.CH.Geo.dll")]
  public class GeoConditionTests
  {
    public GeoConditionTests()
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
    }

    private ExpressionParserManager CreateGeoProvider(string requestIP, bool isInternal = false, bool useMockProxy = false, string spoofip = null)
    {
      MockHttpRequest request = new MockHttpRequest("http://blue.com?qaspoofip=" + spoofip ?? string.Empty);

      IPAddress address;
      if (IPAddress.TryParse(requestIP, out address))
      {
        request.MockRemoteAddress(address);
      }

      MockHttpContext.SetFromWorkerRequest(request);

      MockProviderContainer container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      container.RegisterProvider<IGeoProvider, GeoProvider>();
      container.SetMockSetting(MockSiteContextSettings.IsRequestInternal, isInternal);

      ExpressionParserManager result = new ExpressionParserManager(container);
      result.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;
      return result;
    }

    [TestMethod]
    public void ClientCountryAny()
    {
      string expression = "clientCountryAny(us)";
      ExpressionParserManager parser = CreateGeoProvider("127.0.0.1");
      bool result = parser.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ClientCountryAnyFalse()
    {
      string expression = "clientCountryAny(au)";
      ExpressionParserManager parser = CreateGeoProvider("127.0.0.1");
      bool result = parser.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void ClientRegionIsEU()
    {
      string expression = "clientRegionIs(2,EU)";
      ExpressionParserManager parser = CreateGeoProvider("5.158.255.220", false);
      bool result = parser.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ClientRegionIsNotEU()
    {
      string expression = "clientRegionIs(2,EU)";
      ExpressionParserManager parser = CreateGeoProvider("1.179.3.3", false);
      bool result = parser.EvaluateExpression(expression);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void ClientCityAny()
    {
      string expression = "clientCityAny([Kansas City],Scottsdale)";
      ExpressionParserManager parser = CreateGeoProvider("97.74.104.201", false);
      bool result = parser.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ClientPostalCodeAny()
    {
      string expression = "clientPostalCodeAny(85260,85254)";
      ExpressionParserManager parser = CreateGeoProvider("97.74.104.201", false);
      bool result = parser.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void ClientMetroCodeAny()
    {
      string expression = "clientMetroCodeAny(752,753)";
      ExpressionParserManager parser = CreateGeoProvider("97.74.104.201", false);
      bool result = parser.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }


  }
}
