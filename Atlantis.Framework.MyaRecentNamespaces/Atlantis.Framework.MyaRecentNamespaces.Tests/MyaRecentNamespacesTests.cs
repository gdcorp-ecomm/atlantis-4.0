using System;
using Atlantis.Framework.MyaRecentNamespaces.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaRecentNamespaces.Tests
{
  [TestClass]
  public class MyaRecentNamespacesTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BasicTest()
    {
      string shopperId = "832652";
      DateTime lookBackTo = DateTime.Now.AddDays(-1);
      MyaRecentNamespacesRequestData request = new MyaRecentNamespacesRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, lookBackTo);
      MyaRecentNamespacesResponseData response = Engine.Engine.ProcessRequest(request, 449) as MyaRecentNamespacesResponseData;
      Assert.IsTrue(response.Namespaces.Count > -1);
    }
  }
}
