using Atlantis.Framework.Providers.Shopper.Interface;
using Atlantis.Framework.Providers.Shopper.Tests.Properties;
using Atlantis.Framework.Shopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  [TestClass]
  public class ShopperDataFieldsTests
  {
    [TestMethod]
    public void ValidateShopperDataFields()
    {
      Type shopperDataFieldsType = typeof(ShopperDataFields);
      HashSet<string> dataFieldNames = new HashSet<string>();

      var constantFields = shopperDataFieldsType.GetFields(BindingFlags.Static | BindingFlags.Public);
      foreach (var fieldConstant in constantFields)
      {
        if ((fieldConstant.IsLiteral) && (!fieldConstant.IsInitOnly))
        {
          string value = fieldConstant.GetValue(null) as string;
          dataFieldNames.Add(value);
        }
      }

      Assert.AreNotEqual(0, dataFieldNames.Count);

      var request = new GetShopperRequestData("832652", "1.1.1.1", "unittest", dataFieldNames);
      string requestXml = request.ToXML();
      ValidateShopperGetXml(requestXml);
    }

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
  }
}
