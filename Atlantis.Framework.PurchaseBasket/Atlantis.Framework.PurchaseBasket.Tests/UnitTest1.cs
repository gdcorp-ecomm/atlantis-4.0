using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.PurchaseBasket.Interface;

namespace Atlantis.Framework.PurchaseBasket.Tests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      string filePath = @"C:\Delta\services\atlantisframework\4.0\Atlantis.Framework.PurchaseBasket\Atlantis.Framework.PurchaseBasket.Tests\ExamplePurchase.xml";
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);
      System.IO.StreamReader fileStream=new System.IO.StreamReader(filePath);
      string purchaseXML = fileStream.ReadToEnd();
      oData.PopulateRequestFromXML(purchaseXML);
    }

    [TestMethod]
    public void TestMethod2()
    {
      string filePath = @"C:\Delta\services\atlantisframework\4.0\Atlantis.Framework.PurchaseBasket\Atlantis.Framework.PurchaseBasket.Tests\ExamplePurchase.xml";
      PurchaseBasketRequestData oData = new PurchaseBasketRequestData("75866", "www.yahoo.com", string.Empty, string.Empty, 0);
      System.IO.StreamReader fileStream = new System.IO.StreamReader(filePath);
      string purchaseXML = "<PaymentInformation pathway=\"50ad538f-9086-452c-a867-cb8438af65f4\" translationLanguage=\"en\"><BillingInfo street1=\"14455 N Hayden Rd\" street2=\"Suite 219\" city=\"Scottsdale\" email=\"jburroughs@godaddy.com\" first_name=\"Quality\" last_name=\"Assurance\" phone1=\"4805058800\" zip=\"85260\" country=\"us\" state=\"AZ\" /><PaymentOrigin order_billing=\"domestic\" _repversion=\"\" entered_by=\"customer\" _webserver=\"LT127323-TBIRD\" from_app=\"\" order_source=\"Online\" remote_addr=\"127.0.0.1\" remote_host=\"127.0.0.1\" currencyDisplay=\"USD\" /><Payments><Profile pp_shopperProfileID=\"38505\" /><ISCPayment amount=\"0\" /><GiftCardPayment account_number=\"7020422783202408\" /><GiftCardPayment account_number=\"7028352643717208\" /></Payments></PaymentInformation>";
      oData.PopulateRequestFromXML(purchaseXML);
    }
  }
}
