using System;
using Atlantis.Framework.VanityHost.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.VanityHost.Tests
{
  [TestClass]
  public class VanityHostTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("VanityHost.xml")]
    [DeploymentItem("Atlantis.Framework.VanityHost.Impl.dll")]
    public void GetVanityHosts()
    {
      VanityHostRequestData request = new VanityHostRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      VanityHostResponseData response = (VanityHostResponseData)DataCache.DataCache.GetProcessRequest(request, 526);

      Assert.IsNotNull(response.VanityHostItems);
    }
  }
}
