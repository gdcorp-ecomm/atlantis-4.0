using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockLocalization;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.CH.Localization.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.CH.Localization.dll")]
  public class IsGlobalSiteTests
  {
    private ExpressionParserManager SetContainerAndConditionHandler(string countrySite)
    {
      MockProviderContainer mockProviderContainer = new MockProviderContainer();
      mockProviderContainer.SetData(MockLocalizationProviderSettings.CountrySite, countrySite);
      mockProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      mockProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      mockProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      mockProviderContainer.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();

      ExpressionParserManager result = new ExpressionParserManager(mockProviderContainer);
      result.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;
      return result;
    }

    [TestMethod]
    public void IsGlobalSiteTrue()
    {
      var expression = "isGlobalSite()";
      var expressionParser = SetContainerAndConditionHandler("www");
      Assert.IsTrue(expressionParser.EvaluateExpression(expression));
    }

    [TestMethod]
    public void IsGlobalSiteFalse()
    {
      var expression = "isGlobalSite()";
      var expressionParser = SetContainerAndConditionHandler("au");
      Assert.IsFalse(expressionParser.EvaluateExpression(expression));
    }

  }
}
