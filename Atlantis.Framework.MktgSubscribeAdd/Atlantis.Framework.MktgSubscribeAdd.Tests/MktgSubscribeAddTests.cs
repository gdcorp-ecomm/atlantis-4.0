using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MktgSubscribeAdd.Interface;

namespace Atlantis.Framework.MktgSubscribeAdd.Tests
{
  [TestClass]
  public class MktgSubscribeAddTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AddSubscriber()
    {
      MktgSubscribeAddRequestData request = new MktgSubscribeAddRequestData("850774",
                                                                            "http://www.MktgSubscribeAddTests.com/", 
                                                                            string.Empty, 
                                                                            Guid.NewGuid().ToString(),
                                                                            0, 
                                                                            "sthota@godaddy.com", 
                                                                            25, 
                                                                            1, 
                                                                            0, 
                                                                            "Sridhar", 
                                                                            "Thota", 
                                                                            false, 
                                                                            "172.68.2.12", 
                                                                            "Atlantis Unit Tests");

      MktgSubscribeAddResponseData response = (MktgSubscribeAddResponseData)Engine.Engine.ProcessRequest(request, 169);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AddEmailPushSubscriber()
    {
      MktgSubscribeAddRequestData request = new MktgSubscribeAddRequestData("847235",
                                                                            "http://www.MktgSubscribeAddTests.com/",
                                                                            string.Empty,
                                                                            Guid.NewGuid().ToString(),
                                                                            0,
                                                                            "trwalker@godaddy.com",
                                                                            44,
                                                                            1,
                                                                            0,
                                                                            "Tim",
                                                                            "Walker",
                                                                            true,
                                                                            "172.68.2.12",
                                                                            "Atlantis Unit Tests");

      MktgSubscribeAddResponseData response = (MktgSubscribeAddResponseData)Engine.Engine.ProcessRequest(request, 169);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
