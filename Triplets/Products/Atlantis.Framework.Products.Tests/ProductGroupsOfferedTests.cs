using Atlantis.Framework.Products.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class ProductGroupsOfferedTests
  {
    [TestMethod]
    public void ProductGroupsOfferedCacheKeySame()
    {
      ProductGroupsOfferedRequestData request = new ProductGroupsOfferedRequestData(1);
      ProductGroupsOfferedRequestData request2 = new ProductGroupsOfferedRequestData(1);
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void ProductGroupsOfferedCacheKeyDiffernet()
    {
      ProductGroupsOfferedRequestData request = new ProductGroupsOfferedRequestData(1);
      ProductGroupsOfferedRequestData request2 = new ProductGroupsOfferedRequestData(2);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void ProductGroupsOfferedResponseCount()
    {
      string productGroupOfferedXml = "<data><item pl_productGroupID=\"1\" description=\"Web Hosting\"/></data>";
      ProductGroupsOfferedResponseData response = ProductGroupsOfferedResponseData.FromCacheXml(productGroupOfferedXml);
      Assert.AreEqual(1, response.Count);
      Assert.IsNull(response.GetException());
      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void ProductGroupsOfferedResponseIsOffered()
    {
      string productGroupOfferedXml = "<data><item pl_productGroupID=\"1\" description=\"Web Hosting\"/></data>";
      ProductGroupsOfferedResponseData response = ProductGroupsOfferedResponseData.FromCacheXml(productGroupOfferedXml);
      Assert.IsTrue(response.IsOffered(1));
      Assert.IsFalse(response.IsOffered(30));
    }

    [TestMethod]
    public void ProductGroupsOfferedResponseBadGroup()
    {
      string productGroupOfferedXml = "<data><item pl_productGroupID=\"A1\" description=\"Web Hosting\"/></data>";
      ProductGroupsOfferedResponseData response = ProductGroupsOfferedResponseData.FromCacheXml(productGroupOfferedXml);
      Assert.AreEqual(0, response.Count);
      Assert.IsFalse(response.IsOffered(1));
    }

    [TestMethod]
    public void ProductGroupsOfferedResponseNoGroup()
    {
      string productGroupOfferedXml = "<data><item description=\"Web Hosting\"/></data>";
      ProductGroupsOfferedResponseData response = ProductGroupsOfferedResponseData.FromCacheXml(productGroupOfferedXml);
      Assert.AreEqual(0, response.Count);
      Assert.IsFalse(response.IsOffered(1));
    }


    const int _REQUESTTYPE = 701;

    [TestMethod]
    public void ProducGroupsPrivateLabel2()
    {
      ProductGroupsOfferedRequestData request = new ProductGroupsOfferedRequestData(2);
      ProductGroupsOfferedResponseData response = (ProductGroupsOfferedResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.IsOffered(30));
      Assert.IsTrue(response.IsOffered(1));
    }

  }
}
