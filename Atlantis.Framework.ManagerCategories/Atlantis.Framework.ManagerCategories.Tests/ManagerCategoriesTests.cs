using Atlantis.Framework.ManagerCategories.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.ManagerCategories.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.ManagerCategories.Impl.dll")]
  public class ManagerCategoriesTests
  {
    int _REQUESTTYPE = 462;

    [TestMethod]
    public void GetManagerCategories()
    {
      ManagerCategoriesRequestData request = new ManagerCategoriesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2055);
      ManagerCategoriesResponseData response = (ManagerCategoriesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(0, response.ManagerCategoryCount);
    }

    [TestMethod]
    public void ManagerCategoriesRequestProperties()
    {
      ManagerCategoriesRequestData request = new ManagerCategoriesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2055);
      Assert.AreEqual(2055, request.ManagerUserId);
      Assert.AreEqual("2055", request.GetCacheMD5());
      Assert.IsTrue(request.ToXML().Contains("2055"));
      XElement.Parse(request.ToXML());
    }

    const string _TESTRESPONSEXML = "<user name=\"hello\"><category>44</category><category>45</category></user>";
    const string _NESTEDTESTRESPONSEXML = "<root><user name=\"hello\"><category>44</category><category>45</category></user></root>";

    [TestMethod]
    public void ManagerCatgoriesResponseNoUserInXml()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(_NESTEDTESTRESPONSEXML);
      string name;
      Assert.IsTrue(response.TryGetManagerAttribute("Name", out name));
      Assert.AreEqual("hello", name);

      Assert.IsFalse(response.TryGetManagerAttribute("blue", out name));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseNull()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(null);
      Assert.AreEqual(ManagerCategoriesResponseData.Empty, response);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseEmpty()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(string.Empty);
      Assert.AreEqual(ManagerCategoriesResponseData.Empty, response);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseBadXml()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml("<parsethis!");
      Assert.AreEqual(ManagerCategoriesResponseData.Empty, response);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseTryGetAttribute()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      string name;
      Assert.IsTrue(response.TryGetManagerAttribute("Name", out name));
      Assert.AreEqual("hello", name);

      Assert.IsFalse(response.TryGetManagerAttribute("blue", out name));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseHasAttribute()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      Assert.IsTrue(response.HasManagerAttribute("Name"));
      Assert.IsFalse(response.HasManagerAttribute("blue"));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseAttributeKeys()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      Assert.AreNotEqual(0, response.ManagerAttributeKeys.Count);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseToXml()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void ManagerCategoriesResponseEmpty()
    {
      Assert.AreEqual(0, ManagerCategoriesResponseData.Empty.ManagerAttributeKeys.Count);
      Assert.AreEqual(0, ManagerCategoriesResponseData.Empty.ManagerCategoryCount);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseCategoryCount()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      Assert.AreEqual(2, response.ManagerCategoryCount);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseCategoryHasAll()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      int[] categories = new int[2] { 44, 45 };
      Assert.IsTrue(response.HasAllManagerCategories(categories));
      Assert.IsTrue(response.HasAnyManagerCategories(categories));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseCategoryHasAny()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      int[] categories = new int[2] { 44, 79 };
      Assert.IsFalse(response.HasAllManagerCategories(categories));
      Assert.IsTrue(response.HasAnyManagerCategories(categories));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseCategoryHas()
    {
      ManagerCategoriesResponseData response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      Assert.IsFalse(response.HasManagerCategory(77));
      Assert.IsTrue(response.HasManagerCategory(44));
    }

    [TestMethod]
    public void InvalidRequestData()
    {
      InvalidRequestData request = new InvalidRequestData();
      ManagerCategoriesResponseData response = (ManagerCategoriesResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(ManagerCategoriesResponseData.Empty, response);
    }

  }
}
