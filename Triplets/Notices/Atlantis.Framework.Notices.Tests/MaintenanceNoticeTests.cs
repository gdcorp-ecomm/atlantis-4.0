using Atlantis.Framework.Notices.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.Notices.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Notices.Impl.dll")]
  public class MaintenanceNoticeTests
  {

    // <data count="1"><item maintID="46" maintHeader="IDP - new notice" maintText="This is a notice just for IDP." maintOn="1" /></data>

    [TestMethod]
    public void RequestDataProperties()
    {
      var request = new MaintenanceNoticeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "idp");
      Assert.AreEqual("idp", request.WebSite);
      Assert.AreEqual("IDP", request.GetCacheMD5());

      var request2 = new MaintenanceNoticeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "iDp");
      Assert.AreEqual(request2.GetCacheMD5(), request.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataPropertiesNull()
    {
      var request = new MaintenanceNoticeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null);
      Assert.AreEqual(string.Empty, request.WebSite);
      Assert.AreEqual(string.Empty, request.GetCacheMD5());

      var request2 = new MaintenanceNoticeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty);
      Assert.AreEqual(request2.GetCacheMD5(), request.GetCacheMD5());
    }

    [TestMethod]
    public void ResponseDataNoNoticeIsOff()
    {
      Assert.IsFalse(MaintenanceNoticeResponseData.NoNotice.IsNoticeOn);
      Assert.IsNull(MaintenanceNoticeResponseData.NoNotice.GetException());
    }

    [TestMethod]
    public void ResponseDataFromNull()
    {
      var response = MaintenanceNoticeResponseData.FromCacheDataXml(null);
      Assert.AreEqual(MaintenanceNoticeResponseData.NoNotice, response);
    }

    [TestMethod]
    public void ResponseDataFromEmpty()
    {
      var response = MaintenanceNoticeResponseData.FromCacheDataXml(string.Empty);
      Assert.AreEqual(MaintenanceNoticeResponseData.NoNotice, response);
    }

    [TestMethod]
    public void ResponseDataFromBadXml()
    {
      var response = MaintenanceNoticeResponseData.FromCacheDataXml("<data>hello");
      Assert.AreEqual(MaintenanceNoticeResponseData.NoNotice, response);
    }

    [TestMethod]
    public void ResponseDataNoticeOff()
    {
      string data = "<data count=\"1\"><item maintID=\"46\" maintHeader=\"IDP - new notice\" maintText=\"This is a notice just for IDP.\" maintOn=\"0\" /></data>";
      var response = MaintenanceNoticeResponseData.FromCacheDataXml(data);
      Assert.AreEqual(MaintenanceNoticeResponseData.NoNotice, response);
    }

    [TestMethod]
    public void ResponseDataNoNotice()
    {
      string data = "<data count=\"0\"></data>";
      var response = MaintenanceNoticeResponseData.FromCacheDataXml(data);
      Assert.AreEqual(MaintenanceNoticeResponseData.NoNotice, response);
    }

    [TestMethod]
    public void ResponseDataWithNotice()
    {
      string data = "<data count=\"1\"><item maintID=\"46\" maintHeader=\"IDP - new notice\" maintText=\"This is a notice just for IDP.\" maintOn=\"1\" /></data>";
      var response = MaintenanceNoticeResponseData.FromCacheDataXml(data);
      Assert.AreNotEqual(MaintenanceNoticeResponseData.NoNotice, response);

      Assert.IsTrue(response.IsNoticeOn);
      Assert.AreEqual("IDP - new notice", response.NoticeHeader);
      Assert.AreEqual("This is a notice just for IDP.", response.NoticeBody);
      Assert.IsNull(response.GetException());

      XElement.Parse(response.ToXML());
    }

    const int _REQUESTTYPE = 683;

    [TestMethod] 
    public void RunRequestWithNull()
    {
      var request = new MaintenanceNoticeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null);
      var response = (MaintenanceNoticeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(MaintenanceNoticeResponseData.NoNotice, response);
    }

    [TestMethod]
    public void RunRequestWithNoData()
    {
      var request = new MaintenanceNoticeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "nowebsitenamewillwork");
      var response = (MaintenanceNoticeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(MaintenanceNoticeResponseData.NoNotice, response);
    }

    [TestMethod]
    public void RunRequestWithValidData()
    {
      var request = new MaintenanceNoticeRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "idp");
      var response = (MaintenanceNoticeResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);

      // NOTE: if idp message is not on in dev this test will not pass... find another notice that is on.
      Assert.IsTrue(response.IsNoticeOn);
    }


  }
}
