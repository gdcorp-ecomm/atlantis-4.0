using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeValidation.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeValidation.Impl.dll")]
  public class DotTypeValidationTests
  {
    [TestMethod]
    public void TestMethod1()
    {
    }
  }
}
