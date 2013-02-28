using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.SsoServiceProvider.Interface;

namespace Atlantis.Framework.SsoServiceProvider.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.SsoServiceProvider.Impl.dll")]
  public class SsoServiceProviderTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void ValidSpKey()
    {
      SsoServiceProviderRequestData request = new SsoServiceProviderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "GDSWNET-D1WSDV-MMICCO");
      SsoServiceProviderResponseData response = (SsoServiceProviderResponseData)DataCache.DataCache.GetProcessRequest(request, 484);
      Assert.AreEqual(SsoServiceProviderStatus.Active, response.Status);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void InvalidSpKey()
    {
      SsoServiceProviderRequestData request = new SsoServiceProviderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "GDSWNET-NotThere");
      SsoServiceProviderResponseData response = (SsoServiceProviderResponseData)DataCache.DataCache.GetProcessRequest(request, 484);
      Assert.AreEqual(SsoServiceProviderStatus.NotFound, response.Status);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void GetPrimarySpkey()
    {
      SsoServiceProviderRequestData request = new SsoServiceProviderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "BRCARTNET-G1DWCARTWEB001");
      SsoServiceProviderResponseData response = (SsoServiceProviderResponseData)DataCache.DataCache.GetProcessRequest(request, 484);

      Assert.IsTrue(response.IsUsingPrimaryServiceProviderName);
      Assert.AreEqual(response.PrimaryServiceProviderName, "BRCARTNET-G1CARTWEB");
    }

  }
}
