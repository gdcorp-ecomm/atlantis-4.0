using System.Collections.Generic;
using Atlantis.Framework.RegGetDotTypeRegistrar.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RegGetDotTypeRegistrar.Tests
{
  [TestClass]
  public class GetDotTypeRegistrarTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetDotTypeRegistrar()
    {
      RegGetDotTypeRegistrarRequestData request = new RegGetDotTypeRegistrarRequestData("77311",
        string.Empty, string.Empty, string.Empty, 0, new List<string>(){ "com", "net", "co.uk" });
      RegGetDotTypeRegistrarResponseData response
        = (RegGetDotTypeRegistrarResponseData)Engine.Engine.ProcessRequest(request, 281);
      string responseXML = response.ToXML();
      Assert.IsTrue(response.IsValid);
    }
  }
}
