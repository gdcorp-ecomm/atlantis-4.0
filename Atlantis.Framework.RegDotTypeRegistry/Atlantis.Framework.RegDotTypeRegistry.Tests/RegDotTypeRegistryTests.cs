using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.RegDotTypeRegistry.Interface;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Xml.Schema;
using System;

namespace Atlantis.Framework.RegDotTypeRegistry.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Impl.dll")]
  public class RegDotTypeRegistryTests
  {
    private RegDotTypeRegistryResponseData GetRegistryData(string dotType)
    {
      RegDotTypeRegistryRequestData request = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, dotType);
      RegDotTypeRegistryResponseData response = (RegDotTypeRegistryResponseData)DataCache.DataCache.GetProcessRequest(request, 639);
      return response;
    }

    [TestMethod]
    public void GetRegistryDataComAu()
    {
      RegDotTypeRegistryResponseData comAU = GetRegistryData("com.au");
      Assert.IsNotNull(comAU);
    }

    [TestMethod]
    public void GetRegistryDataCoUk()
    {
      RegDotTypeRegistryResponseData comAU = GetRegistryData("co.uk");
      Assert.IsNotNull(comAU);
    }

    [TestMethod]
    public void HasRegistrationRegistryId()
    {
      RegDotTypeRegistryResponseData com = GetRegistryData("com");
      Assert.IsNotNull(com.RegistrationAPI.Id);
    }

    [TestMethod]
    public void HasTransferRegistryId()
    {
      RegDotTypeRegistryResponseData com = GetRegistryData("com");
      Assert.IsNotNull(com.TransferAPI.Id);
    }

    [TestMethod]
    public void DefaultTimeout()
    {
      var request = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com");
      Assert.AreEqual(TimeSpan.FromSeconds(10), request.RequestTimeout);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void NullDotType()
    {
      var request = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EmptyDotType()
    {
      var request = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, string.Empty);
    }

    [TestMethod]
    public void GetRegistryDataCacheKeyCaseInsensitive()
    {
      RegDotTypeRegistryRequestData requestLower = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com.au");
      RegDotTypeRegistryRequestData requestUpper = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "COM.AU");
      Assert.AreEqual(requestLower.GetCacheMD5(), requestUpper.GetCacheMD5());
    }

    [TestMethod]
    public void GetRegistryDataCacheKeyUnique()
    {
      RegDotTypeRegistryRequestData requestLower = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com.au");
      RegDotTypeRegistryRequestData requestUpper = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "COM");
      Assert.AreNotEqual(requestLower.GetCacheMD5(), requestUpper.GetCacheMD5());
    }

    [TestMethod]
    public void ValidRequestXml()
    {
      RegDotTypeRegistryRequestData request = new RegDotTypeRegistryRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "NET");
      string xml = request.ToXML();
      XDocument requestElement = XDocument.Parse(xml);

      XmlSchemaSet schemas = new XmlSchemaSet();
      schemas.Add(LoadXSD("RequestFormat.xsd"));

      requestElement.Validate(schemas, null);
    }

    private XmlSchema LoadXSD(string xsdName)
    {
      XmlSchema result;

      string resourcePath = "Atlantis.Framework.RegDotTypeRegistry.Tests." + xsdName;
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
      {
        result = XmlSchema.Read(textReader, null);
      }

      return result;
    }


  }
}
