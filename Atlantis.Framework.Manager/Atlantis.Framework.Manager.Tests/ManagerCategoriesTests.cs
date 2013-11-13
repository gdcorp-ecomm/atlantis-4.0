using System.Xml;
using System.Xml.Linq;
using Atlantis.Framework.Manager.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Manager.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Manager.Impl.dll")]
  public class ManagerCategoriesTests
  {
    private const string _TESTRESPONSEXML = "<user name=\"hello\"><category>44</category><category>45</category></user>";
    private const string _NESTEDTESTRESPONSEXML = "<root><user name=\"hello\"><category>44</category><category>45</category></user></root>";
    private const int _REQUESTTYPE = 462;

    [TestMethod]
    public void GetManagerCategories()
    {
      var request = new ManagerCategoriesRequestData(2055);
      var response = (ManagerCategoriesResponseData) Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      string blah;
      response.TryGetManagerAttribute("login_name", out blah);
      Assert.AreNotEqual(0, response.ManagerCategoryCount);
    }

    [TestMethod]
    public void ManagerCategoriesRequestProperties()
    {
      var request = new ManagerCategoriesRequestData(2055);
      Assert.AreEqual(2055, request.ManagerUserId);
      Assert.AreEqual("2055", request.GetCacheMD5());
    }

    [TestMethod]
    public void ManagerCatgoriesResponseNoUserInXml()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(_NESTEDTESTRESPONSEXML);
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
    [ExpectedException(typeof(XmlException))]
    public void ManagerCatgoriesResponseBadXml()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml("<parsethis!");
      Assert.AreEqual(ManagerCategoriesResponseData.Empty, response);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseTryGetAttribute()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      string name;
      Assert.IsTrue(response.TryGetManagerAttribute("Name", out name));
      Assert.AreEqual("hello", name);

      Assert.IsFalse(response.TryGetManagerAttribute("blue", out name));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseHasAttribute()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      Assert.IsTrue(response.HasManagerAttribute("Name"));
      Assert.IsFalse(response.HasManagerAttribute("blue"));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseAttributeKeys()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      Assert.AreNotEqual(0, response.ManagerAttributeKeys.Count);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseToXml()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
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
      var response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      Assert.AreEqual(2, response.ManagerCategoryCount);
    }

    [TestMethod]
    public void ManagerCatgoriesResponseCategoryHasAll()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      var categories = new int[2] {44, 45};
      Assert.IsTrue(response.HasAllManagerCategories(categories));
      Assert.IsTrue(response.HasAnyManagerCategories(categories));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseCategoryHasAny()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      var categories = new int[2] {44, 79};
      Assert.IsFalse(response.HasAllManagerCategories(categories));
      Assert.IsTrue(response.HasAnyManagerCategories(categories));
    }

    [TestMethod]
    public void ManagerCatgoriesResponseCategoryHas()
    {
      var response = ManagerCategoriesResponseData.FromCacheDataXml(_TESTRESPONSEXML);
      Assert.IsFalse(response.HasManagerCategory(77));
      Assert.IsTrue(response.HasManagerCategory(44));
    }
  }


}