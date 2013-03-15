using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MSALoginUser.Interface;
using Atlantis.Framework.MSALoginUser.Impl;

namespace Atlantis.Framework.MSALoginUser.Tests
{
  [TestClass]
  public class MSALoginUserTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MSALoginUser.Impl.dll")]
    public void MSALoginUserTest()
    {
      MSALoginUserRequestData request = new MSALoginUserRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, "andy@alinetech.com", "abc123", string.Empty);
      MSALoginUserResponseData response = (MSALoginUserResponseData) Engine.Engine.ProcessRequest(request, 670);      
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
