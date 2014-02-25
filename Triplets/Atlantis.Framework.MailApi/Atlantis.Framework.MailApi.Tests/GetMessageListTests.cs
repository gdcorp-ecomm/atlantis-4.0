using Atlantis.Framework.MailApi.Interface;
using Atlantis.Framework.MailApi.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MailApi.Tests
{
  [TestClass]
  public class GetMessageListTests
  {
    [TestMethod]
    public void FolderNumProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "mailBaseUrl", "appKey");
      Assert.AreEqual("folderNum", request.FolderNum);
    }

    [TestMethod]
    public void OffsetProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "mailBaseUrl", "appKey");
      Assert.AreEqual("offset", request.Offset);
    }

    [TestMethod]
    public void CountProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "mailBaseUrl", "appKey");
      Assert.AreEqual("count", request.Count);
    }

    [TestMethod]
    public void FilterProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "mailBaseUrl", "appKey");
      Assert.AreEqual("filter", request.Filter);
    }

    [TestMethod]
    public void MailHashProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "mailBaseUrl", "appKey");
      Assert.AreEqual("mailHash", request.MailHash);
    }

    [TestMethod]
    public void MailBaseUrl()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "mailBaseUrl", "appKey");
      Assert.AreEqual("mailBaseUrl", request.MailBaseUrl);
    }

    [TestMethod]
    public void AppKeyProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "mailBaseUrl", "appKey");
      Assert.AreEqual("appKey", request.AppKey);
    }

    [TestMethod]
    public void MailHeaderShouldSerializeIsSecureFalse()
    {
      var response = GetMessageListResponseData.FromJsonData(Resources.ValidGetMessageListData);

      var mailHeader = response.MessageListData.MailHeaderList[0];
      Assert.IsFalse(mailHeader.ShouldSerializeIsSecure());
    }

    [TestMethod]
    public void MailHeaderShouldSerializeIsSecureTrue()
    {
      var response = GetMessageListResponseData.FromJsonData(Resources.ValidGetMessageListData);

      var mailHeader = response.MessageListData.MailHeaderList[1];
      Assert.IsTrue(mailHeader.ShouldSerializeIsSecure());
    }

    [TestMethod]
    public void MockFromJsonData()
    {
      var response = GetMessageListResponseData.FromJsonData(Resources.ValidGetMessageListData);

      var mailHeader = response.MessageListData.MailHeaderList[0];
      
      Assert.AreEqual(318877174, mailHeader.HeaderNum);
      Assert.AreEqual(821720, mailHeader.FolderNum);
      Assert.AreEqual(13095, mailHeader.MsgUid);
      Assert.AreEqual("1393271690.981742.p3plgemini04-06.prod.phx.4060105024", mailHeader.Filename);
      Assert.AreEqual("tester@qa-emailpod04.com", mailHeader.ToFld);
      Assert.AreEqual("tester qa-emailp", mailHeader.ToSort);
      Assert.AreEqual("", mailHeader.Cc);
      Assert.AreEqual("Aaron Smentkowski <asmenter15@gmail.com>", mailHeader.FromFld);
      Assert.AreEqual("aaron smentkowsk", mailHeader.FromSort);
      Assert.AreEqual("", mailHeader.ReplyTo);
      Assert.AreEqual(1393271690, mailHeader.InternalDate);
      Assert.AreEqual(1393271683, mailHeader.MsgDate);
      Assert.AreEqual("Test", mailHeader.Subject);
      Assert.AreEqual("test", mailHeader.SubjectSort);
      Assert.AreEqual(2144, mailHeader.MsgSize);
      Assert.AreEqual("2014-02-24 12:55:00", mailHeader.ModifiedDate);
      Assert.AreEqual(3, mailHeader.Priority);
      Assert.IsFalse(mailHeader.HasAttachment);
      Assert.IsFalse(mailHeader.IsAnswered);
      Assert.IsFalse(mailHeader.IsDraft);
      Assert.IsFalse(mailHeader.IsForwarded);
      Assert.AreEqual("<CAH8Ns+sxn4s+Ci874GYMbwvD6CTO-tGVkOo12B2hpJNxyBfEGA@mail.gmail.com>", mailHeader.MessageId);
      Assert.IsFalse(mailHeader.Flagged);
      Assert.IsFalse(mailHeader.Preferred);
      Assert.IsTrue(mailHeader.Read);
      Assert.IsFalse(mailHeader.Recallable);
      Assert.IsFalse(mailHeader.IsSecure);

      Assert.AreEqual(5, response.MessageListData.MailHeaderList.Count);
    }
  }
}
