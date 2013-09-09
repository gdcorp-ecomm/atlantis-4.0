using Atlantis.Framework.Shopper.Interface;
using Atlantis.Framework.Shopper.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
  public class SearchShoppersTests
  {
    private void ValidateShopperSearchXml(string requestXml)
    {
      using (TextReader reader = new StringReader(Resources.ShopperSearch))
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
    public void SearchShoppersRequestDataPropertiesBasic()
    {
      var searchFields = new Dictionary<string, string>();
      searchFields["first_name"] = "Michael";
      var request = new SearchShoppersRequestData("1.2.3.4", "unittest", searchFields);

      Assert.AreEqual("1.2.3.4", request.OriginIpAddress);
      Assert.AreEqual("unittest", request.RequestedBy);
      ValidateShopperSearchXml(request.ToXML());
    }

    [TestMethod]
    public void SearchShoppersRequestDataPropertiesWithFields()
    {
      var searchFields = new Dictionary<string, string>();
      searchFields["first_name"] = "Michael";
      var returnFields = new[] { "first_name", "last_name" };

      var request = new SearchShoppersRequestData("1.2.3.4", "unittest", searchFields, returnFields);
      ValidateShopperSearchXml(request.ToXML());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SearchShoppersRequestDataPropertiesNullSearchFieldDictionary()
    {
      var request = new SearchShoppersRequestData("1.2.3.4", "unittest", null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SearchShoppersRequestDataPropertiesNullSearchFields()
    {
      var searchFields = new Dictionary<string, string>();
      searchFields[string.Empty] = "Michael";
      searchFields["first_name"] = null;

      var request = new SearchShoppersRequestData("1.2.3.4", "unittest", searchFields);
    }

    private const string _ONESHOPPER = "<ShopperSearchReturn><Shopper shopper_id=\"01z\"/></ShopperSearchReturn>";

    [TestMethod]
    public void SearchShopperResponseDataBasic()
    {
      var response = SearchShoppersResponseData.FromShopperSearchXml(_ONESHOPPER);
      Assert.AreEqual(1, response.Count);
      var shopper = response.ShoppersFound.First();
      Assert.AreEqual(1, shopper.Count);
      Assert.AreEqual(ShopperResponseStatusType.Success, response.Status.Status);
      XElement.Parse(response.ToXML());
    }


    private const int _REQUESTTYPE = 740;

    [TestMethod]
    public void SearchShoppersRequestNoReturnFields()
    {
      var searchFields = new Dictionary<string, string>();
      searchFields["first_name"] = "Michael";

      var request = new SearchShoppersRequestData("1.2.3.4", "unitest", searchFields);
      var response = (SearchShoppersResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(0, response.Count);
      var firstShopper = response.ShoppersFound.First();
      Assert.AreEqual(1, firstShopper.Count);
    }

    [TestMethod]
    public void SearchShoppersRequestWithReturnFields()
    {
      var searchFields = new Dictionary<string, string>();
      searchFields["first_name"] = "Michael";

      var returnFields = new[] { "first_name", "last_name" };

      var request = new SearchShoppersRequestData("1.2.3.4", "unitest", searchFields, returnFields);
      var response = (SearchShoppersResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreNotEqual(0, response.Count);
      var firstShopper = response.ShoppersFound.First();
      Assert.AreEqual(3, firstShopper.Count);
    }

    [TestMethod]
    public void SearchShoppersRequestWithInvalidReturnFields()
    {
      var searchFields = new Dictionary<string, string>();
      searchFields["first_name"] = "Michael";

      var returnFields = new[] { "first_name", "nickname" };

      var request = new SearchShoppersRequestData("1.2.3.4", "unitest", searchFields, returnFields);
      var response = (SearchShoppersResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(ShopperResponseStatusType.InvalidRequestField, response.Status.Status);
    }

    [TestMethod]
    public void SearchShoppersRequestWithInvalidSearchFields()
    {
      var searchFields = new Dictionary<string, string>();
      searchFields["nickname"] = "Michael";

      var request = new SearchShoppersRequestData("1.2.3.4", "unitest", searchFields);
      var response = (SearchShoppersResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(ShopperResponseStatusType.InvalidRequestField, response.Status.Status);
    }

  }
}
