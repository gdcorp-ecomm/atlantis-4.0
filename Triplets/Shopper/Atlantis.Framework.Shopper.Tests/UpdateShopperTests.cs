using Atlantis.Framework.Shopper.Interface;
using Atlantis.Framework.Shopper.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Atlantis.Framework.Shopper.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class UpdateShopperTests
  {
    private void ValidateShopperUpdateXml(string requestXml)
    {
      using (TextReader reader = new StringReader(Resources.ShopperUpdate))
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
    public void UpdateShopperRequestDataPropertiesBasic()
    {
      var request = new UpdateShopperRequestData("832652", "1.2.3.4", "unittest");
      Assert.AreEqual("832652", request.ShopperID);
      Assert.AreEqual("1.2.3.4", request.OriginIpAddress);
      Assert.AreEqual("unittest", request.RequestedBy);
      
      ValidateShopperUpdateXml(request.ToXML());
    }

    [TestMethod]
    public void UpdateShopperRequestDataPropertiesWithUpdateFields()
    {
      Dictionary<string, string> updatefields = new Dictionary<string, string>();
      updatefields["city"] = "Springfield";
      updatefields["street2"] = null;

      var request = new UpdateShopperRequestData("832652", "1.2.3.4", "unittest", updatefields);

      ValidateShopperUpdateXml(request.ToXML());
      XElement updateXml = XElement.Parse(request.ToXML());
      var fields = updateXml.Descendants("Field");
      Assert.AreEqual(2, fields.Count());
    }

    [TestMethod]
    public void UpdateShopperRequestAddFields()
    {
      var request = new UpdateShopperRequestData("832652", "1.2.3.4", "unittest");
      request.AddField("first_name", "Mechanic");

      Dictionary<string, string> updates = new Dictionary<string,string>();
      updates["last_name"] = "Micco";
      updates["city"] = "Springfield";
      updates["first_name"] = "Cleaner";
      request.AddFields(updates);

      XElement requestXml = XElement.Parse(request.ToXML());
      var fieldElements = requestXml.Descendants("Field");
      Assert.AreEqual(3, fieldElements.Count());
    }

    [TestMethod]
    public void UpdateShopperRequestAddInvalidFields()
    {
      var request = new UpdateShopperRequestData("832652", "1.2.3.4", "unittest");
      request.AddField(string.Empty, "Mechanic");
      request.AddField(null, "value");
      request.AddField("hello", null);

      XElement requestXml = XElement.Parse(request.ToXML());
      var fieldElements = requestXml.Descendants("Field");
      Assert.AreEqual(0, fieldElements.Count());
    }

    private const string _EMPTYERROR = "<Status><Error /><Description /></Status>";

    [TestMethod]
    public void UpdateShopperResponseDataProperties()
    {
      var response = UpdateShopperResponseData.FromShopperXml(_EMPTYERROR);
      Assert.AreEqual("Unknown", response.Status.ErrorCode);
    }

    private const int _REQUESTTYPE = 739;

    [TestMethod]
    public void UpdateShopperRequestBasic()
    {
      Dictionary<string, string> updatefields = new Dictionary<string, string>();
      updatefields["city"] = "Springfield";

      var request = new UpdateShopperRequestData("832652", "1.2.3.4", "unittest", updatefields);
      var response = (UpdateShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(ShopperResponseStatusType.Success, response.Status.Status);
      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void UpdateShopperRequestInvalidShopperId()
    {
      Dictionary<string, string> updatefields = new Dictionary<string, string>();
      updatefields["city"] = "Springfield";

      var request = new UpdateShopperRequestData("invalid", "1.2.3.4", "unittest", updatefields);
      var response = (UpdateShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      
      // TODO verify success always comes back from this
      Assert.AreEqual(ShopperResponseStatusType.ShopperNotFound, response.Status.Status);
    }

    [TestMethod]
    public void UpdateShopperRequestInvalidField()
    {
      Dictionary<string, string> updatefields = new Dictionary<string, string>();
      updatefields["township"] = "Springfield";

      var request = new UpdateShopperRequestData("832652", "1.2.3.4", "unittest", updatefields);
      var response = (UpdateShopperResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(ShopperResponseStatusType.InvalidRequestField, response.Status.Status);
    }
  }
}
