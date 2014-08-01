using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.PurchaseBasket.Interface;
using System.Xml;

namespace Atlantis.Framework.PurchaseBasket.Tests
{
  [TestClass]
  [DeploymentItem("ExamplePurchase.xml")]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      string filePath = System.IO.Directory.GetCurrentDirectory() + "\\ExamplePurchase.xml";
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);
      System.IO.StreamReader fileStream=new System.IO.StreamReader(filePath);
      string purchaseXML = fileStream.ReadToEnd();
      oData.PopulateRequestFromXML(purchaseXML);
    }

    [TestMethod]
    public void TestMethod2()
    {
      string filePath = System.IO.Directory.GetCurrentDirectory() + "\\ExamplePurchase.xml";
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);
      System.IO.StreamReader fileStream = new System.IO.StreamReader(filePath);
      string purchaseXML = "<PaymentInformation pathway=\"50ad538f-9086-452c-a867-cb8438af65f4\" translationLanguage=\"en\"><BillingInfo street1=\"14455 N Hayden Rd\" street2=\"Suite 219\" city=\"Scottsdale\" email=\"jburroughs@godaddy.com\" first_name=\"Quality\" last_name=\"Assurance\" phone1=\"4805058800\" zip=\"85260\" country=\"us\" state=\"AZ\" /><PaymentOrigin order_billing=\"domestic\" _repversion=\"\" entered_by=\"customer\" _webserver=\"LT127323-TBIRD\" from_app=\"\" order_source=\"Online\" remote_addr=\"127.0.0.1\" remote_host=\"127.0.0.1\" currencyDisplay=\"USD\" /><Payments><Profile pp_shopperProfileID=\"38505\" /><ISCPayment amount=\"0\" /><GiftCardPayment account_number=\"7020422783202408\" /><GiftCardPayment account_number=\"7028352643717208\" /></Payments></PaymentInformation>";
      oData.PopulateRequestFromXML(purchaseXML);
    }

    [TestMethod]
    public void AddInstantPurchaseAttribute()
    {
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);

      oData.AddInstantPurchaseAttribute("name", "value");
      
      Assert.AreEqual(1, oData.InstantPurchaseAttributes.Count);
      Assert.AreEqual("value", oData.InstantPurchaseAttributes["name"]);
    }

    [TestMethod]
    public void AddInstantPurchaseItem()
    {
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);

      oData.AddInstantPurchaseItem("<item />");

      Assert.AreEqual(1, oData.InstantItemElements.Count);
      Assert.AreEqual("<item />", oData.InstantItemElements[0]);
    }

    [TestMethod]
    public void GenerateXmlNoInstantPurchase()
    {
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);

      string results = oData.ToXML();

      Assert.IsFalse(results.Contains("</PurchaseBasketWithItems>"));
      Assert.IsFalse(results.Contains("</InstantPurchase>"));
      Assert.IsFalse(results.Contains("</itemRequest>"));
    }

    [TestMethod]
    public void GenerateXmlWithInstantPurchase()
    {
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);
      oData.AddInstantPurchaseAttribute("estimateOnly", "1");
      oData.AddInstantPurchaseItem("<item productid=\"3646\" />");

      string results = oData.ToXML();

      Assert.IsTrue(results.Contains("</PurchaseBasketWithItems>"));
      Assert.IsTrue(results.Contains("</InstantPurchase>"));
      Assert.IsTrue(results.Contains("</itemRequest>"));
    }

    [TestMethod]
    public void GenerateXmlWithInstantPurchaseAttributes()
    {
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);
      oData.AddInstantPurchaseAttribute("estimateOnly", "1");
      oData.AddInstantPurchaseItem("<item productid=\"3646\" />");

      string results = oData.ToXML();
      XmlDocument purchaseDoc = new XmlDocument();
      purchaseDoc.LoadXml(results);
      XmlNode instantNode = purchaseDoc.SelectSingleNode("//InstantPurchase");

      Assert.IsNotNull(instantNode);
      Assert.IsNotNull(instantNode.Attributes);
      Assert.AreEqual(1, instantNode.Attributes.Count);
      Assert.AreEqual("estimateOnly", instantNode.Attributes[0].Name);
      Assert.AreEqual("1", instantNode.Attributes[0].Value);
    }

    [TestMethod]
    public void GenerateXmlWithInstantPurchaseItems()
    {
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);
      oData.AddInstantPurchaseAttribute("estimateOnly", "1");
      oData.AddInstantPurchaseItem("<item productid=\"3646\" />");

      string results = oData.ToXML();
      XmlDocument purchaseDoc = new XmlDocument();
      purchaseDoc.LoadXml(results);
      XmlNode instantNode = purchaseDoc.SelectSingleNode("//InstantPurchase/itemRequest");

      Assert.IsNotNull(instantNode);
      Assert.IsNotNull(instantNode.ChildNodes);
      Assert.AreEqual(1, instantNode.ChildNodes.Count);
      Assert.AreEqual("<item productid=\"3646\" />", instantNode.ChildNodes[0].OuterXml);
    }

    public void PopulateRequestFromXMLWithInstantPurchase()
    {
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);

      oData.PopulateRequestFromXML("<PurchaseBasketWithItems><PaymentInformation><Payments /></PaymentInformation><InstantPurchase estimateOnly=\"1\"><itemRequest><item productid=\"3646\" /></itemRequest></InstantPurchase></PurchaseBasketWithItems>");
      string results = oData.ToXML();
      XmlDocument purchaseDoc = new XmlDocument();
      purchaseDoc.LoadXml(results);
      XmlNode instantNode = purchaseDoc.SelectSingleNode("//InstantPurchase/itemRequest");

      Assert.IsNotNull(instantNode);
      Assert.IsNotNull(instantNode.ChildNodes);
      Assert.AreEqual(1, instantNode.ChildNodes.Count);
      Assert.AreEqual("<item productid=\"3646\" />", instantNode.ChildNodes[0].OuterXml);
    }
  }
}
