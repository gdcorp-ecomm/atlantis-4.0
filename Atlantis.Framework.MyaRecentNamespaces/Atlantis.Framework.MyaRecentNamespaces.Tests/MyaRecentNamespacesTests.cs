using System;
using System.Diagnostics;
using Atlantis.Framework.MyaRecentNamespaces.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
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
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      string shopperId = "856907";
      DateTime lookBackTo = DateTime.Now.AddDays(-1);
      MyaRecentNamespacesRequestData request = new MyaRecentNamespacesRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, lookBackTo);
      
      MyaRecentNamespacesResponseData response1 = SessionCache.SessionCache.GetProcessRequest<MyaRecentNamespacesResponseData>(request, 449);
      MyaRecentNamespacesResponseData response2 = SessionCache.SessionCache.GetProcessRequest<MyaRecentNamespacesResponseData>(request, 449);

      Debug.WriteLine(response1.ToXML());
      Assert.IsTrue(response1.Namespaces.Count > -1);
      Assert.AreEqual(response1.ToXML(), response2.ToXML());
    }
  }
}
