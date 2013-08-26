using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Atlantis.Framework.Shopper.Interface;
using Atlantis.Framework.Shopper.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
      Assert.AreEqual(0, request.InterestPreferences.Count());
      Assert.AreEqual(0, request.CommunicationPreferences.Count());

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

    [TestMethod]
    public void InterestPreferenceProperties()
    {
      var pref = new InterestPreference(1, 3);
      Assert.AreEqual(1, pref.InterestTypeId);
      Assert.AreEqual(3, pref.CommunicationTypeId);
    }

    [TestMethod]
    public void AddInterestPreferencesToRequest()
    {
      var pref = new InterestPreference(1, 2);
      var pref2 = new InterestPreference(2, 3);
      var request = new GetShopperRequestData("832652", "1.2.3.4", "unittest");
      request.AddInterestPreference(pref);
      request.AddInterestPreference(pref2);
      Assert.AreEqual(2, request.InterestPreferences.Count());

      ValidateShopperGetXml(request.ToXML());
    }

    [TestMethod]
    public void AddCommunicationPreferencesToRequest()
    {
      var request = new GetShopperRequestData("832652", "1.2.3.4", "unittest");
      request.AddCommunicationPreference(1);
      request.AddCommunicationPreference(2);
      request.AddCommunicationPreference(1);
      Assert.AreEqual(2, request.CommunicationPreferences.Count());

      ValidateShopperGetXml(request.ToXML());
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
    }

    private const string _SHOPPER_RESPONSE_COMMPREFERENCE =
      "<Shopper ID=\"822497\"><Preferences><Communication CommTypeID=\"200\" OptIn=\"0\"/><Communication CommTypeID=\"100\" OptIn=\"1\"/></Preferences></Shopper>";

    [TestMethod]
    public void GetShopperResponseDataPropertiesCommunication()
    {
      var response = GetShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_COMMPREFERENCE);
      Assert.IsFalse(response.HasFieldValue("gdshop_shopper_payment_type_id"));
      int optIn200 = response.GetCommunicationPreference(200);
      int optIn100 = response.GetCommunicationPreference(100);
      Assert.AreEqual(0, optIn200);
      Assert.AreEqual(1, optIn100);
    }

    private const string _SHOPPER_RESPONSE_INTEREST =
      "<Shopper ID=\"822497\"><Preferences><Interest CommTypeID=\"3\" InterestTypeID=\"5\" OptIn=\"15\"/><Interest CommTypeID=\"2\" InterestTypeID=\"2\" OptIn=\"0\"/></Preferences></Shopper>";

    [TestMethod]
    public void GetShopperResponseDataPropertiesInterest()
    {
      var response = GetShopperResponseData.FromShopperXml(_SHOPPER_RESPONSE_INTEREST);
      Assert.IsFalse(response.HasFieldValue("gdshop_shopper_payment_type_id"));
      int optIn22 = response.GetInterestPreference(2, 2);
      int optIn35 = response.GetInterestPreference(3, 5);
      Assert.AreEqual(0, optIn22);
      Assert.AreEqual(15, optIn35);
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
