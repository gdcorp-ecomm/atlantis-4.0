using System.Xml.Linq;
using Atlantis.Framework.Manager.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Manager.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Manager.Impl.dll")]
  public class ManagerCategoriesTests
  {
    private const string TESTRESPONSEXML = "<user name=\"hello\"><category>44</category><category>45</category></user>";
    private const string NESTEDTESTRESPONSEXML = "<root><user name=\"hello\"><category>44</category><category>45</category></user></root>";
    private const int REQUESTTYPE = 462;

    [TestMethod]
    public void GetManagerCategories()
    {
      var request = new ManagerCategoriesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2055);
      var response = (ManagerCategoriesResponseData) Engine.Engine.ProcessRequest(request, REQUESTTYPE);
      string blah;
      response.TryGetManagerAttribute("login_name", out blah);
      response.ToXML();
      Assert.AreNotEqual(0, response.ManagerCategoryCount);
    }

    [TestMethod]
    public void ManagerCategoriesRequestProperties()
    {
      var request = new ManagerCategoriesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2055);
      Assert.AreEqual(2055, request.ManagerUserId);
      Assert.AreEqual("2055", request.GetCacheMD5());
      Assert.IsTrue(request.ToXML().Contains("2055"));
      XElement.Parse(request.ToXML());
    }

    [TestMethod]
    public void ManagerCatgoriesResponseNoUserInXml()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(NESTEDTESTRESPONSEXML);
      string name;
      Assert.IsTrue(response.TryGetManagerAttribute("Name", out name));
      Assert.AreEqual("hello", name);

      Assert.IsFalse(response.TryGetManagerAttribute("blue", out name));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseNull()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(null);
      Assert.AreEqual(ManagerCategoriesResponseData.Empty, response);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseEmpty()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(string.Empty);
      Assert.AreEqual(ManagerCategoriesResponseData.Empty, response);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseBadXml()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml("<parsethis!");
      Assert.AreEqual(ManagerCategoriesResponseData.Empty, response);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseTryGetAttribute()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(TESTRESPONSEXML);
      string name;
      Assert.IsTrue(response.TryGetManagerAttribute("Name", out name));
      Assert.AreEqual("hello", name);

      Assert.IsFalse(response.TryGetManagerAttribute("blue", out name));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseHasAttribute()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(TESTRESPONSEXML);
      Assert.IsTrue(response.HasManagerAttribute("Name"));
      Assert.IsFalse(response.HasManagerAttribute("blue"));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseAttributeKeys()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(TESTRESPONSEXML);
      Assert.AreNotEqual(0, response.ManagerAttributeKeys.Count);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseToXml()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(TESTRESPONSEXML);
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
      var response = ManagerCategoriesResponseData.FromCacheDataXml(TESTRESPONSEXML);
      Assert.AreEqual(2, response.ManagerCategoryCount);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseCategoryHasAll()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(TESTRESPONSEXML);
      var categories = new int[2] {44, 45};
      Assert.IsTrue(response.HasAllManagerCategories(categories));
      Assert.IsTrue(response.HasAnyManagerCategories(categories));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseCategoryHasAny()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(TESTRESPONSEXML);
      var categories = new int[2] {44, 79};
      Assert.IsFalse(response.HasAllManagerCategories(categories));
      Assert.IsTrue(response.HasAnyManagerCategories(categories));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseCategoryHas()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(TESTRESPONSEXML);
      Assert.IsFalse(response.HasManagerCategory(77));
      Assert.IsTrue(response.HasManagerCategory(44));
    }

    [TestMethod]
    public void ManagerCategoriesInvalidRequestData()
    {
      var request = new ManagerCategoriesInvalidRequestData();
      var response = (ManagerCategoriesResponseData) Engine.Engine.ProcessRequest(request, REQUESTTYPE);
      Assert.AreEqual(ManagerCategoriesResponseData.Empty, response);
    }
  }


}