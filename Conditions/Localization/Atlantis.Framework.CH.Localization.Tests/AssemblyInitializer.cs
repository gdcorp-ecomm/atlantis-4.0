using Atlantis.Framework.Conditions.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Atlantis.Framework.CH.Localization.Tests
{
  [TestClass]
  public class AssemblyInitializer
  {
    [AssemblyInitialize]
    public static void Initialize(TestContext testContext)
    {
      ConditionHandlerManager.AutoRegisterConditionHandlers(Assembly.GetExecutingAssembly());
    }
  }
}
