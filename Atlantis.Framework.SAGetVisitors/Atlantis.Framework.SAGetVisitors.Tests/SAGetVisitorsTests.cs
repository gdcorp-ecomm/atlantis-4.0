using System;
using System.Diagnostics;
using Atlantis.Framework.SAGetVisitors.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SAGetVisitors.Tests
{
  [TestClass]
  public class SAGetVisitorsTests
  {
    private const string _shopperId = "4916";

    private TestContext testContextInstance;
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    [TestMethod]
    [DeploymentItem("Atlantis.config")]
    public void TestMethod1()
    {
      string domain = "danicaracing.com";
      DateTime startDate = new DateTime(2011, 3, 11);
      DateTime endDate = new DateTime(2011, 3, 17);


      int _requestType = 354;

      SAGetVisitorsRequestData request = new SAGetVisitorsRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , domain
         , startDate
         , endDate);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      SAGetVisitorsResponseData response = (SAGetVisitorsResponseData)Engine.Engine.ProcessRequest(request, _requestType);


      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(_shopperId == response.ShopperId);

      Debug.WriteLine(response.ToXML());
    }
  }
}
