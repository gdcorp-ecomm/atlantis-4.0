using System;
using Atlantis.Framework.MktgSubscribeRemove.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MktgSubscribeRemove.Tests
{
  [TestClass]
  public class MktgSubscribeRemoveTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void RemoveSubscriber()
    {
      MktgSubscribeRemoveRequestData request = new MktgSubscribeRemoveRequestData("850774",
                                                                                  "http://www.MktgSubscribeRemoveTests.com",
                                                                                  string.Empty,
                                                                                  Guid.NewGuid().ToString(),
                                                                                  1, 
                                                                                  "sthota@godaddy.com", 
                                                                                  25, 
                                                                                  1, 
                                                                                  "172.19.72.107",
                                                                                  "Atlantis Unit Tests");

      MktgSubscribeRemoveResponseData response = (MktgSubscribeRemoveResponseData)Engine.Engine.ProcessRequest(request, 282);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
