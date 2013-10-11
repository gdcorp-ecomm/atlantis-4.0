using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Basket.Tests.Properties;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using Atlantis.Framework.Basket.Interface;

namespace Atlantis.Framework.Basket.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Basket.Impl.dll")]
  public class BasketAddTests
  {
    private void ValidateBasketAddXml(string requestXml)
    {
      using (TextReader reader = new StringReader(Resources.ItemRequest))
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
    public void BasketAddRequestDataProperties()
    {
      var request = new BasketAddRequestData("832652");
      Assert.AreEqual("832652", request.ShopperID);
      ValidateBasketAddXml(request.ToXML());
    }

    [TestMethod]
    public void BasketAddRequestDataSetRequestAttribute()
    {
      var request = new BasketAddRequestData("832652");
      request.SetItemRequestAttribute("isc", "testisc");
      ValidateBasketAddXml(request.ToXML());

      var requestElement = XElement.Parse(request.ToXML());
      var iscAttribute = requestElement.Attribute("isc");
      Assert.AreEqual("testisc", iscAttribute.Value);
    }

    [TestMethod]
    public void BasketAddRequestDataSetRequestAttributeTwice()
    {
      var request = new BasketAddRequestData("832652");
      request.SetItemRequestAttribute("isc", "testisc");
      request.SetItemRequestAttribute("isc", "testisc2");
      ValidateBasketAddXml(request.ToXML());

      var requestElement = XElement.Parse(request.ToXML());
      var count = requestElement.Attributes("isc").Count();
      Assert.AreEqual(1, count);

      var iscAttribute = requestElement.Attribute("isc");
      Assert.AreEqual("testisc2", iscAttribute.Value);
    }

    [TestMethod]
    public void BasketAddRequestDataAddElement()
    {
      var request = new BasketAddRequestData("832652");
      var itemElement = new XElement("item");
      request.AddRequestElement(itemElement);
      ValidateBasketAddXml(request.ToXML());
    }

    [TestMethod]
    public void BasketAddRequestDataAddMultipleItems()
    {
      var request = new BasketAddRequestData("832652");
      var itemElement = new XElement("item");
      request.AddRequestElement(itemElement);
      var itemElement2 = new XElement("item");
      request.AddRequestElement(itemElement2);
      ValidateBasketAddXml(request.ToXML());

      var requestElement = XElement.Parse(request.ToXML());
      int count = requestElement.Elements("item").Count();
      Assert.AreEqual(2, count);
    }

    [TestMethod]
    public void BasketAddResponseDataPropertiesOneError()
    {
      var response = BasketAddResponseData.FromResponseXml(Resources.InvalidShopperResponse);
      Assert.IsTrue(response.Status.HasErrors);
      Assert.AreEqual(1, response.Status.Errors.Count());
      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void BasketAddResponseDataPropertiesSuccess()
    {
      var response = BasketAddResponseData.FromResponseXml(Resources.SuccessResponse);
      Assert.IsFalse(response.Status.HasErrors);
      Assert.AreEqual(BasketResponseStatusType.Success, response.Status.Status);
    }

    const int _REQUESTTYPE = 746;

    [TestMethod]
    public void BasketAddRequestError()
    {
      var request = new BasketAddRequestData(string.Empty);
      var response = (BasketAddResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
    }

    [TestMethod]
    public void BasketAddRequestSuccess()
    {
      var request = new BasketAddRequestData("832652");
      var response = (BasketAddResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
    }

  }
}
