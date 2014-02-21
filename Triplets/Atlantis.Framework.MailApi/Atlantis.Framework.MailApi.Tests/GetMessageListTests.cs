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
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailBaseUrl");
      Assert.AreEqual("folderNum", request.FolderNum);
    }

    [TestMethod]
    public void OffsetProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailBaseUrl");
      Assert.AreEqual("offset", request.Offset);
    }

    [TestMethod]
    public void CountProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailBaseUrl");
      Assert.AreEqual("count", request.Count);
    }

    [TestMethod]
    public void FilterProperty()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailBaseUrl");
      Assert.AreEqual("filter", request.Filter);
    }

    [TestMethod]
    public void MailBaseUrl()
    {
      var request = new GetMessageListRequestData("folderNum", "offset", "count", "filter", "mailBaseUrl");
      Assert.AreEqual("mailBaseUrl", request.MailBaseUrl);
    }
  }
}
