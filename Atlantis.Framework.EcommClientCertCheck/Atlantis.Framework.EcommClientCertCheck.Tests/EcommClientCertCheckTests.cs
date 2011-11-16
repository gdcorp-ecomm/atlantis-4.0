using System;
using Atlantis.Framework.EcommClientCertCheck.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommClientCertCheck.Tests
{
  [TestClass]
  public class EcommClientCertCheckTests
  {
    private const string APPLICATION_NAME = "mobile:PushNotification";
    private const string BAD_APPLICATION_NAME = "asfjkljk:dsfsfasffsdfssfd";

    private const string METHOD_NAME = "PushMessage";
    private const string BAD_METHOD_NAME = "SendRequest";

    private const string CERTIFICATE_SUBJECT = "CN=corp.web.mob.dev.client.godaddy.com, OU=Domain Control Validated, O=corp.web.mob.dev.client.godaddy.com";
    private const string UN_AUTH_CERTIFICATE_SUBJECT = "CN=corp.web.sales.dev.client.godaddy.com, OU=Domain Control Validated, O=corp.web.sales.dev.client.godaddy.com";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthorizedClientCheck()
    {
      EcommClientCertCheckRequestData request = new EcommClientCertCheckRequestData(CERTIFICATE_SUBJECT,
                                                                                    APPLICATION_NAME,
                                                                                    METHOD_NAME,
                                                                                    "847235",
                                                                                    "http://www.EcommClientCertCheckTests.com/",
                                                                                    string.Empty,
                                                                                    Guid.NewGuid().ToString(),
                                                                                    1);

      EcommClientCertCheckResponeData responeData = (EcommClientCertCheckResponeData)Engine.Engine.ProcessRequest(request, 450);

      Assert.IsTrue(responeData.IsSuccess);
      Assert.IsTrue(responeData.IsAuthorized);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthorizedClientCheckDatacache()
    {
      EcommClientCertCheckRequestData request = new EcommClientCertCheckRequestData(CERTIFICATE_SUBJECT,
                                                                                    APPLICATION_NAME,
                                                                                    METHOD_NAME,
                                                                                    "847235",
                                                                                    "http://www.EcommClientCertCheckTests.com/",
                                                                                    string.Empty,
                                                                                    Guid.NewGuid().ToString(),
                                                                                    1);

      EcommClientCertCheckResponeData responeData = (EcommClientCertCheckResponeData)DataCache.DataCache.GetProcessRequest(request, 450);

      // Try a second time to get a cached response
      responeData = (EcommClientCertCheckResponeData)DataCache.DataCache.GetProcessRequest(request, 450);

      Assert.IsTrue(responeData.IsSuccess);
      Assert.IsTrue(responeData.IsAuthorized);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void UnAuthorizedClientCertCheck()
    {
      EcommClientCertCheckRequestData request = new EcommClientCertCheckRequestData(UN_AUTH_CERTIFICATE_SUBJECT,
                                                                                    APPLICATION_NAME,
                                                                                    METHOD_NAME,
                                                                                    "847235",
                                                                                    "http://www.EcommClientCertCheckTests.com/",
                                                                                    string.Empty,
                                                                                    Guid.NewGuid().ToString(),
                                                                                    1);

      EcommClientCertCheckResponeData responeData = (EcommClientCertCheckResponeData)Engine.Engine.ProcessRequest(request, 450);

      Assert.IsTrue(responeData.IsSuccess);
      Assert.IsFalse(responeData.IsAuthorized);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BadMethodNameCheck()
    {
      EcommClientCertCheckRequestData request = new EcommClientCertCheckRequestData(CERTIFICATE_SUBJECT,
                                                                                    APPLICATION_NAME,
                                                                                    BAD_METHOD_NAME,
                                                                                    "847235",
                                                                                    "http://www.EcommClientCertCheckTests.com/",
                                                                                    string.Empty,
                                                                                    Guid.NewGuid().ToString(),
                                                                                    1);

      EcommClientCertCheckResponeData responeData = (EcommClientCertCheckResponeData)Engine.Engine.ProcessRequest(request, 450);

      Assert.IsTrue(responeData.IsSuccess);
      Assert.IsFalse(responeData.IsAuthorized);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BadApplicatioNameCheck()
    {
      EcommClientCertCheckRequestData request = new EcommClientCertCheckRequestData(CERTIFICATE_SUBJECT,
                                                                                    BAD_APPLICATION_NAME,
                                                                                    METHOD_NAME,
                                                                                    "847235",
                                                                                    "http://www.EcommClientCertCheckTests.com/",
                                                                                    string.Empty,
                                                                                    Guid.NewGuid().ToString(),
                                                                                    1);

      EcommClientCertCheckResponeData responeData = (EcommClientCertCheckResponeData)Engine.Engine.ProcessRequest(request, 450);

      Assert.IsTrue(responeData.IsSuccess);
      Assert.IsFalse(responeData.IsAuthorized);
    }
  }
}
