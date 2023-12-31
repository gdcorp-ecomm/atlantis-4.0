﻿using System.Reflection;
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
  public class CountrySiteAnyConditionHandlerTests
  {
    private ExpressionParserManager SetContainerAndConditionHandler()
    {
      MockProviderContainer mockProviderContainer = new MockProviderContainer();
      mockProviderContainer.SetData(MockLocalizationProviderSettings.CountrySite, "www");
      mockProviderContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      mockProviderContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      mockProviderContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      mockProviderContainer.RegisterProvider<ILocalizationProvider, MockLocalizationProvider>();

      ExpressionParserManager result = new ExpressionParserManager(mockProviderContainer);
      result.EvaluateExpressionHandler += ConditionHandlerManager.EvaluateCondition;
      return result;
    }

    [TestMethod]
    public void OneConditionExpression()
    {
      var expressionParser = SetContainerAndConditionHandler();
      string expression = "countrySiteAny(www)";
      bool result = expressionParser.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void MultipleConditionExpression()
    {
      var expressionParser = SetContainerAndConditionHandler();
      string expression = "countrySiteAny(www) && !countrySiteAny(in)";
      bool result = expressionParser.EvaluateExpression(expression);
      Assert.IsTrue(result);
    }
  }
}
