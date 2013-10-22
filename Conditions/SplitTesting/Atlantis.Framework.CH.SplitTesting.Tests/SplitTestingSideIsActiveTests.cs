using System;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SplitTesting;
using Atlantis.Framework.Providers.SplitTesting.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.SplitTesting.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.SplitTesting.Impl.dll")]
  public class SplitTestingSideIsActiveTests
  {
    private bool _conditionHandlersRegistered;

    [TestInitialize]
    public void InitializeTests()
    {
      if (!_conditionHandlersRegistered)
      {
        ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
        ConditionHandlerManager.RegisterConditionHandler(new SplitTestingSideIsActiveConditionHandler());
        _conditionHandlersRegistered = true;
      }
    }

    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          _providerContainer.RegisterProvider<ISplitTestingProvider, SplitTestingProvider>();
        }

        return _providerContainer;
      }
    }

    [TestMethod]
    public void EvaluateValidConditionFalse()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("SplitTestingSideIsActive", new[] { "9999998989", "A" }, ProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionInvalidFalse()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("SplitTestingSideIsActive", new[] { "123Abc", "A" }, ProviderContainer));
    }
  }
}
