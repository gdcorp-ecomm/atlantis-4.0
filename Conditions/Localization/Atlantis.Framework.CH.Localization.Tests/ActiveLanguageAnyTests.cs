using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Render.ExpressionParser;
using Atlantis.Framework.Testing.MockLocalization;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.Localization.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.CH.Localization.dll")]
  public class ActiveLanguageAnyTests
  {
    private ExpressionParserManager SetContainerAndConditionHandler(string fullLanguage)
    {
      MockProviderContainer mockProviderContainer = new MockProviderContainer();
      mockProviderContainer.SetData(MockLocalizationProviderSettings.FullLanguage, fullLanguage);
      mockProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      mockProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      mockProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      mockProviderContainer.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();

      ExpressionParserManager result = new ExpressionParserManager(mockProviderContainer);
      result.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;
      return result;
    }

    [TestMethod]
    public void ActiveLanguageAnySingleFull()
    {
      var expressionParser = SetContainerAndConditionHandler("en-AU");
      Assert.IsTrue(expressionParser.EvaluateExpression("activeLanguageAny([en-au])"));
      Assert.IsFalse(expressionParser.EvaluateExpression("activeLangaugeAny([en-us])"));
    }

    [TestMethod]
    public void ActiveLanguageAnyMultipleFull()
    {
      var expressionParser = SetContainerAndConditionHandler("en-AU");
      Assert.IsTrue(expressionParser.EvaluateExpression("activeLanguageAny([es-mx],[en-au])"));
      Assert.IsFalse(expressionParser.EvaluateExpression("activeLangaugeAny([en-us],[es-mx])"));
    }

    [TestMethod]
    public void ActiveLanguageAnySingleShort()
    {
      var expressionParser = SetContainerAndConditionHandler("en-AU");
      Assert.IsTrue(expressionParser.EvaluateExpression("activeLanguageAny(en)"));
      Assert.IsFalse(expressionParser.EvaluateExpression("activeLangaugeAny(es)"));
    }

    [TestMethod]
    public void ActiveLanguageAnyMultipleShort()
    {
      var expressionParser = SetContainerAndConditionHandler("en-AU");
      Assert.IsTrue(expressionParser.EvaluateExpression("activeLanguageAny(es,en)"));
      Assert.IsFalse(expressionParser.EvaluateExpression("activeLangaugeAny(es,fr)"));
    }


  }
}
