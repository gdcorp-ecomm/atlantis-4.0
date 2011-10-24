using System;
using Atlantis.Framework.GetMobileToken.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetMobileToken.Test
{
  [TestClass]
  public class GetMobileTokenTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMobilTokenTypeBasic()
    {
      GetMobileTokenRequestData request = new GetMobileTokenRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "TestArvind", "password", 1, Guid.NewGuid().ToString());
      
      GetMobileTokenResponseData response = (GetMobileTokenResponseData) Engine.Engine.ProcessRequest(request, 84);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}
