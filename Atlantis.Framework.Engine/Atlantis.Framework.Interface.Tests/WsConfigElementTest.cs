using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Engine.Tests
{

  [TestClass()]
  public class WsConfigElementTest
  {

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(AtlantisException))]
    public void GetClientCertificateTest_Certificate_ConfigValue_Key_NotFound_In_WSConfig()
    {
      string progId = "1";
      string assembly = "doesnt.matter.for.this.test";
      string webServiceUrl = "doesnt.matter.for.this.test";
      Dictionary<string, string> configValues = new Dictionary<string, string>();
      configValues.Add("CertificateName", "corp.web.mya.dev.client.godaddy.com");
      WsConfigElement target = new WsConfigElement(1, progId, assembly, webServiceUrl, configValues);
      string friendlyName = "CertificateNameIsNotCorrect";
      X509Certificate2 actual;
      actual = target.GetClientCertificate(friendlyName);
    }

    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    [ExpectedException(typeof(AtlantisException))]
    public void GetClientCertificateTest_Certificate_NotFound_In_CertificateStore()
    {
      string progId = "1";
      string assembly = "doesnt.matter.for.this.test";
      string webServiceUrl = "doesnt.matter.for.this.test";
      Dictionary<string, string> configValues = new Dictionary<string, string>();
      configValues.Add("CertificateName", "corp.web.mya.dev.client.godaddy.com.not.going.to.be.found");
      WsConfigElement target = new WsConfigElement(1, progId, assembly, webServiceUrl, configValues);

      string friendlyName = "CertificateName";
      X509Certificate2 actual;
      actual = target.GetClientCertificate(friendlyName);

    }


  }
}
