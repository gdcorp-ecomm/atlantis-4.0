using System;
using Atlantis.Framework.MSALoginUser.Interface;
using Atlantis.Framework.MSAGetFolderList.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MSAGetFolderList.Tests
{
  [TestClass]
  public class MSAGetFolderListTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MSALoginUser.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.MSAGetFolderList.Impl.dll")]
    public void TestMethod1()
    {
      MSALoginUserRequestData request = new MSALoginUserRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 1, "andy@alinetech.com", "abc123", string.Empty);
      MSALoginUserResponseData response = (MSALoginUserResponseData)Engine.Engine.ProcessRequest(request, 670);

      MSAGetFolderListRequestData folderRequest = new MSAGetFolderListRequestData(string.Empty, string.Empty, string.Empty,
       string.Empty, 0, response.BaseUrl, response.Hash, string.Empty);
      MSAGetFolderListResponseData folderResponse = (MSAGetFolderListResponseData)Engine.Engine.ProcessRequest(folderRequest, 671);
      Assert.IsTrue(folderResponse.IsSuccess);
    }
  }
}
