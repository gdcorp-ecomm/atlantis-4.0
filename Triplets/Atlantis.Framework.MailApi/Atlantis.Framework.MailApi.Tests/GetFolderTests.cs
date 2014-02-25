using Atlantis.Framework.MailApi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MailApi.Tests
{
  [TestClass]
  public class GetFolderTests
  {
    [TestMethod]
    public void GetFolderRequestData_FolderNum()
    {
      var request = new GetFolderRequestData("folderNum", "session", "appKey", "key", "mailBaseUrl");
      Assert.AreEqual("folderNum",request.FolderNum);
    }

    [TestMethod]
    public void GetFolderRequestData_Session()
    {
      var request = new GetFolderRequestData("folderNum", "session", "appKey", "key", "mailBaseUrl");
      Assert.AreEqual("session", request.Session);
    }

    [TestMethod]
    public void GetFolderRequestData_AppKey()
    {
      var request = new GetFolderRequestData("folderNum", "session", "appKey", "key", "mailBaseUrl");
      Assert.AreEqual("appKey", request.AppKey);
    }

    [TestMethod]
    public void GetFolderRequestData_Key()
    {
      var request = new GetFolderRequestData("folderNum", "session", "appKey", "key", "mailBaseUrl");
      Assert.AreEqual("key", request.Key);
    }

    [TestMethod]
    public void GetFolderRequestData_MailBaseUrl()
    {
      var request = new GetFolderRequestData("folderNum", "session", "appKey", "key", "mailBaseUrl");
      Assert.AreEqual("mailBaseUrl", request.MailBaseUrl);
    }

    [TestMethod]
    public void GetFolderResponseData_EmptyJson()
    {
      var response = GetFolderResponseData.FromJsonData(string.Empty);
      Assert.IsNotNull(response);
      Assert.IsNotNull(response.MailFolder);
    }

  }
}
