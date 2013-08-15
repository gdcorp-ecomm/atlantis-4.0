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
  }
}
