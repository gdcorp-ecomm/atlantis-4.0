using System;
using Atlantis.Framework.DomainContactFields.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainContactFields.Tests
{
  [TestClass]
  public class DomainContactFieldsTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainContactFields.Impl.dll")]
    public void GetDomainContactFieldsTest()
    {
      int requestId = 651;

      var request = new DomainContactFieldsRequestData(string.Empty, "", string.Empty, string.Empty, 1, "com.au");
      request.RequestTimeout = TimeSpan.FromSeconds(4);

      var response = Engine.Engine.ProcessRequest(request, requestId) as DomainContactFieldsResponseData;

      Assert.IsTrue(response != null && !string.IsNullOrEmpty(response.DomainContactFields));
    }
  }
}