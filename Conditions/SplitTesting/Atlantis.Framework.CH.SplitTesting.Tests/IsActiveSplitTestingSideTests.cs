using System;
using System.Reflection;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Containers;
using Atlantis.Framework.Providers.SplitTesting;
using Atlantis.Framework.Providers.SplitTesting.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CH.SplitTesting.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.SplitTesting.Impl.dll")]
  public class IsActiveSplitTestingSideTests
  {
    private bool _conditionHandlersRegistered;

    [TestInitialize]
    public void InitializeTests()
    {
      if (!_conditionHandlersRegistered)
      {
        ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
        ConditionHandlerManager.RegisterConditionHandler(new IsActiveSplitTestingSideConditionHandler());
        _conditionHandlersRegistered = true;
      }
    }

    private IProviderContainer _objectProviderContainer;
    private IProviderContainer ObjectProviderContainer
    {
      get
      {
        if (_objectProviderContainer == null)
        {
          _objectProviderContainer = new ObjectProviderContainer();
          _objectProviderContainer.RegisterProvider<ISplitTestingProvider, SplitTestingProvider>();
        }

        return _objectProviderContainer;
      }
    }

    [TestMethod]
    public void EvaluateValidConditionFalse()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("isActiveSplitTestingSide", new[] { "9999998989", "A" }, ObjectProviderContainer));
    }

    [TestMethod]
    public void EvaluateValidConditionInvalidFalse()
    {
      Assert.IsFalse(ConditionHandlerManager.EvaluateCondition("isActiveSplitTestingSide", new[] { "123Abc", "A" }, ObjectProviderContainer));
    }
  }
}
