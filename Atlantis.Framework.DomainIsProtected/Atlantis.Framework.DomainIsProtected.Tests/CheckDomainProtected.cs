using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DomainIsProtected.Interface;

namespace Atlantis.Framework.DomainIsProtected.Tests
{
  [TestClass]
  public class CheckDomainProtected
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainIsProtected.Impl.dll")]
    public void TestMethod1()
    {

      DomainIsProtectedRequestData request = new DomainIsProtectedRequestData("856907", string.Empty, string.Empty, string.Empty, 0, 1665617);
      DomainIsProtectedResponseData response =
        (DomainIsProtectedResponseData) Engine.Engine.ProcessRequest(request, 675);

      if (response.IsSuccess)
      {
        bool isProtected = response.IsDomainProtected;
      }


    }
  }
}
