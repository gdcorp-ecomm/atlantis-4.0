using Atlantis.Framework.Shopper.Interface;
using Atlantis.Framework.Shopper.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Atlantis.Framework.Shopper.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class CreateShopperTests
  {
    private void ValidateShopperCreateXml(string requestXml)
    {
      using (TextReader reader = new StringReader(Resources.ShopperCreate))
      {
        var requestDoc = XDocument.Parse(requestXml);

        ValidationEventHandler validationHandler = ValidationHandler;
        var schemaSet = new XmlSchemaSet();
        schemaSet.Add(XmlSchema.Read(reader, null));

        requestDoc.Validate(schemaSet, validationHandler);
      }
    }

    private void ValidationHandler(object sender, ValidationEventArgs validationEventArgs)
    {
      Assert.AreNotEqual(XmlSeverityType.Error, validationEventArgs.Severity, validationEventArgs.Message);
      Assert.IsNull(validationEventArgs.Exception);
    }

    [TestMethod]
    public void CreateShopperRequestDataProperties()
    {
      var request = new CreateShopperRequestData(2, "1.1.1.2", "unittest");
      Assert.AreEqual(2, request.PrivateLabelId);
      Assert.AreEqual("unittest", request.RequestedBy);

      ValidateShopperCreateXml(request.ToXML());
    }

    private const string _SHOPPER_RESPONSE_XML = "<Shopper ID=\"822497\"></Shopper>";

    [TestMethod]
    public void CreateShopperResponseDataPropertiesSuccess()
    {
      var response = CreateShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_XML);
      Assert.AreEqual(ShopperResponseStatusType.Success, response.Status.Status);
      Assert.AreEqual("822497", response.ShopperId);
      Assert.IsNull(response.GetException());
      string xml = response.ToXML();
      XElement.Parse(xml);
    }

    private const string _SHOPPER_RESPONSE_UNKNOWN_ERROR = "<Status />";

    [TestMethod]
    public void CreateShopperResponseDataPropertiesFail()
    {
      var response = CreateShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_UNKNOWN_ERROR);
      Assert.AreEqual(ShopperResponseStatusType.UnknownError, response.Status.Status);
      Assert.AreEqual(string.Empty, response.ShopperId);
    }

    private const int _REQUESTTYPE = 738;

    [TestMethod]
    public void CreateShopperRequestBasic()
    {
      var request = new CreateShopperRequestData(1, "1.1.2.3", "unittest");
      var response = (CreateShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(ShopperResponseStatusType.Success, response.Status.Status);
      Assert.IsFalse(string.IsNullOrEmpty(response.ShopperId));
    }

    [TestMethod]
    public void CreateShopperRequestInvaidPrivateLabelId()
    {
      var request = new CreateShopperRequestData(-451, "1.1.2.3", "unittest");
      var response = (CreateShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(ShopperResponseStatusType.Success, response.Status.Status);
      Assert.IsTrue(string.IsNullOrEmpty(response.ShopperId));
    }




  }
}
