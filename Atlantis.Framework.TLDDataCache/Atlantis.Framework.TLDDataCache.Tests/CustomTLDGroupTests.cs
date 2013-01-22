using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class CustomTLDGroupTests
  {
    const int _REQUESTTYPE = 636;

    [TestMethod]
    public void NotFound()
    {
      CustomTLDGroupRequestData request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "TestGroupNotThere748929394829348958");
      CustomTLDGroupResponseData response = (CustomTLDGroupResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTTYPE);
      Assert.IsNotNull(response);
    }

    [TestMethod]
    public void RequestCacheKeyCaseInsenstive()
    {
      CustomTLDGroupRequestData request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "TestGroup1");
      CustomTLDGroupRequestData request2 = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "TestGROUP1");
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void GroupRequestCacheKeyDifferent()
    {
      CustomTLDGroupRequestData request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "TestGroup1");
      CustomTLDGroupRequestData request2 = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "TestGROUP2");
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataToXml()
    {
      string groupName = "TestGroupNotThere748929394829348958";
      CustomTLDGroupRequestData request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, groupName);
      XElement.Parse(request.ToXML());
      Assert.IsTrue(request.ToXML().Contains(groupName.ToUpperInvariant()));
    }

    [TestMethod]
    public void RequestDataGroupNameUpper()
    {
      CustomTLDGroupRequestData request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "lower");
      Assert.AreEqual("LOWER", request.GroupName);
    }

    [TestMethod]
    public void ValidEmptyDefault()
    {
      CustomTLDGroupRequestData request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty);
      CustomTLDGroupResponseData response = (CustomTLDGroupResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(CustomTLDGroupResponseData.EmptyGroup, response);
    }

    [TestMethod]
    public void ValidNoItems()
    {
      CustomTLDGroupRequestData request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "UnitTestNOITEMS");
      CustomTLDGroupResponseData response = (CustomTLDGroupResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(0, response.Count);
    }

    [TestMethod]
    public void ValidOneItem()
    {
      CustomTLDGroupRequestData request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "UnitTestONEITEM");
      CustomTLDGroupResponseData response = (CustomTLDGroupResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(1, response.Count);
    }

    [TestMethod]
    public void ValidManyItems()
    {
      CustomTLDGroupRequestData request = new CustomTLDGroupRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "UnitTestMANYITEMS");
      CustomTLDGroupResponseData response = (CustomTLDGroupResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.Count > 1);
    }

    [TestMethod]
    public void ResponseDataFromNull()
    {
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromDelimitedString(null);
      Assert.AreEqual(CustomTLDGroupResponseData.EmptyGroup, response);
    }

    [TestMethod]
    public void ResponseDataFromEmpty()
    {
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromDelimitedString(string.Empty);
      Assert.AreEqual(CustomTLDGroupResponseData.EmptyGroup, response);
    }

    [TestMethod]
    public void ResponseDataFromEmptyDelimiters()
    {
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromDelimitedString("||||");
      Assert.AreEqual(CustomTLDGroupResponseData.EmptyGroup, response);
    }

    [TestMethod]
    public void ResponseDataFromOneItem()
    {
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromDelimitedString("com");
      Assert.AreNotEqual(CustomTLDGroupResponseData.EmptyGroup, response);
      Assert.AreEqual(1, response.Count);
    }

    [TestMethod]
    public void ResponseDataFromTwoItemsExtraDelimiters()
    {
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromDelimitedString("com|net|");
      Assert.AreNotEqual(CustomTLDGroupResponseData.EmptyGroup, response);
      Assert.AreEqual(2, response.Count);
    }

    [TestMethod]
    public void ResponseDataFromTwoItemsCustomDelimiter()
    {
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromDelimitedString("com&net", '&');
      Assert.AreNotEqual(CustomTLDGroupResponseData.EmptyGroup, response);
      Assert.AreEqual(2, response.Count);
    }


    [TestMethod]
    public void ResponseDataFromTwoItemsExtraDelimitersPacked()
    {
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromDelimitedString("com|||net|");
      Assert.AreNotEqual(CustomTLDGroupResponseData.EmptyGroup, response);
      Assert.AreEqual(2, response.Count);
    }

    [TestMethod]
    public void ResponseDataFromException()
    {
      AtlantisException ex = new AtlantisException("CustomTLDGroupTests.ResponseDataFromException", "0", "Test Error Constructor", string.Empty, null, null);
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromException(ex);
      Assert.IsNotNull(response.GetException());
    }

    [TestMethod]
    public void ResponseDataToXml()
    {
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromDelimitedString("com|net");
      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void ResponseDataTldsInOrder()
    {
      string input = "com|net|org|info";
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromDelimitedString(input);

      string output = string.Join("|", response.TldsInOrder);
      Assert.AreEqual(input, output);
    }

    [TestMethod]
    public void ResponseDataContains()
    {
      string input = "com|net|org|info";
      CustomTLDGroupResponseData response = CustomTLDGroupResponseData.FromDelimitedString(input);
      Assert.IsTrue(response.Contains("ORG"));
      Assert.IsFalse(response.Contains("|info"));
    }

  }
}
