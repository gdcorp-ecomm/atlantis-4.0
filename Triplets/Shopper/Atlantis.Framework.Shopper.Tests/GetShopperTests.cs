using Atlantis.Framework.Shopper.Interface;
using Atlantis.Framework.Shopper.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Atlantis.Framework.Shopper.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class GetShopperTests
  {
    private void ValidateShopperGetXml(string requestXml)
    {
      using (TextReader reader = new StringReader(Resources.ShopperGet))
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
    public void GetShopperRequestDataProperties()
    {
      var request = new GetShopperRequestData("832652", "1.1.1.2", "unittest", new[] {"first_name", "last_name"});
      Assert.AreEqual(2, request.Fields.Count());
      Assert.AreEqual("832652", request.ShopperID);
      Assert.AreEqual("unittest", request.RequestedBy);

      ValidateShopperGetXml(request.ToXML());
    }

    [TestMethod]
    public void GetShopperRequestDataPropertiesNoFields()
    {
      var request = new GetShopperRequestData("832652", "", "unittest");
      Assert.AreEqual(0, request.Fields.Count());
      Assert.AreEqual("832652", request.ShopperID);

      var xml = request.ToXML();
      ValidateShopperGetXml(xml);
      Assert.IsTrue(xml.Contains(Environment.MachineName));
    }

    [TestMethod]
    public void GetShopperRequestDataPropertiesAddSingleFields()
    {
      var request = new GetShopperRequestData("832652", "", "unittest");
      request.AddField("first_name");
      request.AddField("last_name");
      Assert.AreEqual(2, request.Fields.Count());

      var xml = request.ToXML();
      ValidateShopperGetXml(xml);
    }

    [TestMethod]
    public void GetShopperRequestDataPropertiesAddMultipleFields()
    {
      var request = new GetShopperRequestData("832652", "1.1.1.2", "unittest");
      request.AddFields(new[] {"first_name", "last_name"});
      Assert.AreEqual(2, request.Fields.Count());

      ValidateShopperGetXml(request.ToXML());
    }

    [TestMethod]
    public void GetShopperRequestDataPropertiesCaseSensitive()
    {
      var request = new GetShopperRequestData("832652", "1.1.1.2", "unittest", new [] {"first_name", "first_Name"});
      Assert.AreEqual(2, request.Fields.Count());
      Assert.AreEqual("832652", request.ShopperID);
    }

    private const string _SHOPPER_RESPONSE_XML = "<Shopper ID=\"822497\"><Fields><Field Name=\"gdshop_shopper_payment_type_id\">3</Field></Fields></Shopper>";

    [TestMethod]
    public void GetShopperResponseDataProperties()
    {
      var response = GetShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_XML);
      Assert.IsTrue(response.HasFieldValue("gdshop_shopper_payment_type_id"));
      Assert.AreEqual("3", response.GetFieldValue("gdshop_shopper_payment_type_id", string.Empty));
      Assert.IsFalse(response.ToXML().Contains("gdshop_shopper_payment_type_id"));
      Assert.IsNull(response.GetException());
      Assert.AreEqual("822497", response.ShopperId);
      Assert.AreEqual(1, response.Fields.Count());
    }

    private const string _SHOPPER_RESPONSE_UNKNOWN_ERROR = "<Status />";

    [TestMethod]
    public void GetShopperResponseDataUnknownError()
    {
      var response = GetShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_UNKNOWN_ERROR);
      Assert.AreEqual(ShopperResponseStatusType.UnknownError, response.Status.Status);
      Assert.AreEqual(string.Empty, response.Status.ErrorMessage);
      Assert.AreEqual("Unknown", response.Status.ErrorCode);
    }

    private const int _REQUESTTYPE = 735;

    [TestMethod]
    public void GetShopperBasic()
    {
      var request = new GetShopperRequestData("832652", "1.1.1.1", "unittest");
      request.AddField("first_name");
      var response = (GetShopperResponseData) Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.HasFieldValue("first_name"));
    }

    [TestMethod]
    public void ShopperNotFound()
    {
      var request = new GetShopperRequestData("invalidX", "1.1.1.1", "unittest");
      request.AddField("first_name");
      var response = (GetShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(string.Empty, response.ShopperId);
      Assert.AreEqual(ShopperResponseStatusType.ShopperNotFound, response.Status.Status);
      Assert.AreNotEqual(string.Empty, response.Status.ErrorCode);
      Assert.AreNotEqual(string.Empty, response.Status.ErrorMessage);
    }

    [TestMethod]
    public void ShopperBadField()
    {
      var request = new GetShopperRequestData("invalidX", "1.1.1.1", "unittest");
      request.AddField("firstName");
      var response = (GetShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(string.Empty, response.ShopperId);
      Assert.AreEqual(ShopperResponseStatusType.InvalidRequestField, response.Status.Status);
    }

  }
}
