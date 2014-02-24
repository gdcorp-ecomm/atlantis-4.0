using System;
using Atlantis.Framework.MailApi.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MailApi.Tests
{
  [TestClass]
  public class GetMessageListTests
  {
    [TestMethod]
    public void FolderNumProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "appKey");
      Assert.AreEqual("folderNum", request.FolderNum);
    }

    [TestMethod]
    public void OffsetProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "appKey");
      Assert.AreEqual("offset", request.Offset);
    }

    [TestMethod]
    public void CountProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "appKey");
      Assert.AreEqual("count", request.Count);
    }

    [TestMethod]
    public void FilterProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "appKey");
      Assert.AreEqual("filter", request.Filter);
    }

    [TestMethod]
    public void MailHashProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "appKey");
      Assert.AreEqual("mailHash", request.MailHash);
    }

    [TestMethod]
    public void AppKeyProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailHash", "appKey");
      Assert.AreEqual("appKey", request.AppKey);
    }
  }
}
