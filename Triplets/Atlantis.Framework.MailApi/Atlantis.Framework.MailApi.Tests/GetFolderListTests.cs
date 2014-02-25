using System;
using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.MailApi.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MailApi.Tests
{
  [TestClass]
  public class GetFolderListTests
  {
    [TestMethod]
    public void GetFolderListRequestData_AppKey()
    {
      var request = new GetFolderListRequestData("session", "appKey", "key", "mailBaseUrl");
      Assert.AreEqual(request.AppKey, "appKey");
    }

    [TestMethod]
    public void GetFolderListRequestData_Key()
    {
      var request = new GetFolderListRequestData("session", "appKey", "key", "mailBaseUrl");
      Assert.AreEqual(request.Key, "key");
    }

    [TestMethod]
    public void GetFolderListRequestData_Session()
    {
      var request = new GetFolderListRequestData("session", "appKey", "key", "mailBaseUrl");
      Assert.AreEqual(request.Session, "session");
    }

    [TestMethod]
    public void GetFolderListRequestData_BaseUrl()
    {
      var request = new GetFolderListRequestData("session", "appKey", "key", "mailBaseUrl");
      Assert.AreEqual(request.MailBaseUrl, "mailBaseUrl");
    }

    [TestMethod]
    public void GetFolderListResponseData_EmptyJson()
    {
      var response = GetFolderListResponseData.FromJsonData(string.Empty);
      Assert.IsNotNull(response);
      Assert.IsNotNull(response.MailFolders);
    }

    //{"response":[{"folder_num":"821720","folder":"INBOX","user_num":"115748","uid_validity":"1","uid_next":"13096","exists_count":"8442",
    //"seen_count":"99","is_dirty":"0","display_name":"Inbox","system_folder":true}],
    //"state":{"session":"f7fd9744234bf27d3341617cfa7f67e2","app_key":"tv2YfSzBx6zdjHAjIhW9mNe5"}}

    [TestMethod]
    public void MockFromJsonData()
    {

      var response = GetFolderListResponseData.FromJsonData(Resources.ValidGetFolderListData);

      var mailFolder = response.MailFolders[0];

      var state = response.State;

      Assert.AreEqual(821720, mailFolder.FolderNum);
      Assert.AreEqual("INBOX", mailFolder.Folder);
      Assert.AreEqual(115748, mailFolder.UserNum);
      Assert.AreEqual(1, mailFolder.UidValidity);
      Assert.AreEqual(13096, mailFolder.UidNext);
      Assert.AreEqual(8442, mailFolder.ExistsCount);
      Assert.AreEqual(99, mailFolder.SeenCount);
      Assert.IsFalse(mailFolder.IsDirty);
      Assert.AreEqual("Inbox", mailFolder.DisplayName);
      Assert.AreEqual("f7fd9744234bf27d3341617cfa7f67e2", state.Session);
      Assert.AreEqual("tv2YfSzBx6zdjHAjIhW9mNe5", state.AppKey);
      Assert.IsTrue(mailFolder.SystemFolder);
    }
  }
}
