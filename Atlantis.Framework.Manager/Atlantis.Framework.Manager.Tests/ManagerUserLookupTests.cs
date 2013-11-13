using System;
using Atlantis.Framework.Manager.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Manager.Interface;

namespace Atlantis.Framework.Manager.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Manager.Impl.dll")]
  public class ManagerUserLookupTests
  {
    [TestMethod]
    public void ManagerUserLookupRequestDataProperties()
    {
      var request = new ManagerUserLookupRequestData("mmicco");
      Assert.AreEqual("mmicco", request.WindowsUserName);
    }

    [TestMethod]
    public void ManagerUserLookupRequestDataPropertiesNull()
    {
      var request = new ManagerUserLookupRequestData(null);
      Assert.AreEqual(string.Empty, request.WindowsUserName);
    }


    [TestMethod]
    public void ManagerUserLookupCacheKey()
    {
      var request1 = new ManagerUserLookupRequestData("mmicco");
      var request2 = new ManagerUserLookupRequestData("syukna");
      var request3 = new ManagerUserLookupRequestData("Mmicco");
      Assert.AreEqual(request1.GetCacheMD5(), request3.GetCacheMD5());
      Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void ManagerUserLookupResponseDataProperties()
    {
      var response = ManagerUserLookupResponseData.FromCacheDataXml(Resources.ValidUserLookupResponse_txt);
      Assert.AreEqual("Michael", response.FirstName);
      Assert.AreEqual("Micco", response.LastName);
      Assert.AreEqual(2055, response.ManagerUserId);
      Assert.AreEqual("Michael Micco", response.FullName);
      Assert.AreEqual(true, response.IsValid);
    }

    [TestMethod]
    public void ManagerUserLookupResponseDataPropertiesInvalid()
    {
      var response = ManagerUserLookupResponseData.Invalid;
      Assert.AreEqual(string.Empty, response.FirstName);
      Assert.AreEqual(String.Empty, response.LastName);
      Assert.AreEqual(0, response.ManagerUserId);
      Assert.AreEqual(string.Empty, response.FullName);
      Assert.AreEqual(false, response.IsValid);
    }

    [TestMethod]
    public void ManagerUserLookupResponseDataEmpty()
    {
      var response = ManagerUserLookupResponseData.FromCacheDataXml(string.Empty);
      Assert.AreEqual(false, response.IsValid);
      Assert.IsTrue(ReferenceEquals(ManagerUserLookupResponseData.Invalid, response));
    }

    private int _REQUESTTYPE = 65;

    [TestMethod]
    public void GetManagerUser()
    {
      var lookupRequest = new ManagerUserLookupRequestData("mmicco");
      var lookupResponse = (ManagerUserLookupResponseData)Engine.Engine.ProcessRequest(lookupRequest, _REQUESTTYPE);
    }
  }
}
