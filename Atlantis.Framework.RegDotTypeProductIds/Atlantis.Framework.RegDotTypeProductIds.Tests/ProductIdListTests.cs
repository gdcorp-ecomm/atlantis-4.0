using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.RegDotTypeProductIds.Interface;
using System.Xml.Schema;
using System.Reflection;
using System.IO;
using System.Xml.Linq;

namespace Atlantis.Framework.RegDotTypeProductIds.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Impl.dll")]
  public class ProductIdListTests
  {

    private ProductIdListResponseData GetProductIds(string dotType)
    {
      ProductIdListRequestData request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, dotType);
      var result = (ProductIdListResponseData)DataCache.DataCache.GetProcessRequest(request, 640);
      return result;
    }

    [TestMethod]
    public void ProductIdListCom()
    {
      var response = GetProductIds("com");
      Assert.IsNotNull(response);
    }

    [TestMethod]
    public void ProductIdListCacheKeySame()
    {
      ProductIdListRequestData request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com");
      ProductIdListRequestData request2 = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com");
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void ProductIdListCacheKeyDifferent()
    {
      ProductIdListRequestData request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com");
      ProductIdListRequestData request2 = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "net");
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void ValidRequestXml()
    {
      var request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "NET");
      string xml = request.ToXML();
      XDocument requestElement = XDocument.Parse(xml);

      XmlSchemaSet schemas = new XmlSchemaSet();
      schemas.Add(LoadXSD("RequestFormat.xsd"));

      requestElement.Validate(schemas, null);
    }

    private XmlSchema LoadXSD(string xsdName)
    {
      XmlSchema result;

      string resourcePath = "Atlantis.Framework.RegDotTypeProductIds.Tests." + xsdName;
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
      {
        result = XmlSchema.Read(textReader, null);
      }

      return result;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FailedProcessing()
    {
      XElement response = new XElement("response");
      response.Add(new XAttribute("processing", "fail"));
      var responseData = ProductIdListResponseData.FromXElement(response);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void NullDotType()
    {
      var request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EmptyDotType()
    {
      var request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty);
    }

    [TestMethod]
    public void DefaultTimeout()
    {
      var request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com");
      Assert.AreEqual(TimeSpan.FromSeconds(10), request.RequestTimeout);
    }

    [TestMethod]
    public void DotTypeProductTiersRegistration()
    {
      var response = GetProductIds("com");
      DotTypeProductTiers tiers = response.GetDefaultProductTiers(DotTypeProductTypes.Registration);
      Assert.IsNotNull(tiers);
    }

    [TestMethod]
    public void DotTypeProductTiersRegistrationByRegistry()
    {
      var response = GetProductIds("com");
      DotTypeProductTiers tiers = response.GetDefaultProductTiers(DotTypeProductTypes.Registration);

      DotTypeProduct product;
      tiers.TryGetProduct(1, 1, out product);
      Assert.IsNotNull(product);

      DotTypeProductTiers tiers2 = response.GetProductTiersForRegistry(product.RegistryId, DotTypeProductTypes.Registration);
      Assert.IsNotNull(tiers2);
    }

    [TestMethod]
    public void DotTypeProductTiersTransfer()
    {
      var response = GetProductIds("com");
      DotTypeProductTiers tiers = response.GetDefaultProductTiers(DotTypeProductTypes.Transfer);
      Assert.IsNotNull(tiers);
    }

    [TestMethod]
    public void HasDotTypeProductRenewals()
    {
      var response = GetProductIds("com");
      Assert.IsTrue(response.HasProducts(DotTypeProductTypes.Renewal));
    }

  }
}
