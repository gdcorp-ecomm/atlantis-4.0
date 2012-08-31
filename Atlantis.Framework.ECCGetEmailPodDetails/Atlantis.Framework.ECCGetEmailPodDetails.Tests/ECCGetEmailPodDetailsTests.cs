using System;
using Atlantis.Framework.ECCGetEmailPodDetails.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetEmailPodDetails.Tests
{
  [TestClass]
  public class ECCGetEmailPodDetailsTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    [DeploymentItem("Atlantis.Framework.ECCGetEmailPodDetails.Impl.dll")]
    public void EmailPodDetailsValidEmail()
    {
      ECCGetEmailPodDetailsRequestData requestData = new ECCGetEmailPodDetailsRequestData(string.Empty,
                                                                                          "http://Atlantis.Framework.ECCGetEmailPodDetails.Tests.com/",
                                                                                          string.Empty,
                                                                                          Guid.NewGuid().ToString(),
                                                                                          0,
                                                                                          "michaelscott@smccoyforever.com");

      ECCGetEmailPodDetailsResponseData responseData = (ECCGetEmailPodDetailsResponseData)Engine.Engine.ProcessRequest(requestData, 592);

      Assert.IsTrue(responseData.EmailPodDetails.PodId == "0");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    [DeploymentItem("Atlantis.Framework.ECCGetEmailPodDetails.Impl.dll")]
    public void EmailPodDetailsInValidEmail()
    {
      ECCGetEmailPodDetailsRequestData requestData = new ECCGetEmailPodDetailsRequestData(string.Empty,
                                                                                          "http://Atlantis.Framework.ECCGetEmailPodDetails.Tests.com/",
                                                                                          string.Empty,
                                                                                          Guid.NewGuid().ToString(),
                                                                                          0,
                                                                                          "sadfsdfsdffsda@asdfssdfsddaffsd.com");

      ECCGetEmailPodDetailsResponseData responseData = (ECCGetEmailPodDetailsResponseData)Engine.Engine.ProcessRequest(requestData, 592);

      Assert.IsFalse(responseData.IsSuccess);
    }
  }
}
