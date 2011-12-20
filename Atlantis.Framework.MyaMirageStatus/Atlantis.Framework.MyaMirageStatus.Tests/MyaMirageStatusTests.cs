using System;
using Atlantis.Framework.MyaMirageStatus.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaMirageStatus.Tests
{
  [TestClass]
  public class MyaMirageStatusTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BasicTest()
    {
      string shopperId = "alr";
      MyaMirageStatusRequestData request = new MyaMirageStatusRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0);
      MyaMirageStatusResponseData response = Engine.Engine.ProcessRequest(request, 448) as MyaMirageStatusResponseData;
      Assert.IsTrue(response.LastMirageBuild > DateTime.MinValue);
    }
  }
}
