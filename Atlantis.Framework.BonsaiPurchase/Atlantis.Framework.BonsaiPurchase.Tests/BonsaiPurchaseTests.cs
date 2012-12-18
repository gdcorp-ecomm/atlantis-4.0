using System;
using System.Diagnostics;
using Atlantis.Framework.BonsaiPurchase.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.BonsaiPurchase.Tests
{
  [TestClass]
  public class BonsaiPurchaseTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BonsaiPurchaseHostingUpgrade()
    {
      // Note: you will need to change the purchaseXml after each test with EstimateOnly=false
      //string purchaseXml =
      //  "<ClientChange TreeID=\"1295\" ShopperID=\"840420\" " +
      //  "RequestingApp=\"Atlantis.Framework.BonsaiPurchase.Tests\" " +
      //  "RequestingAppHost=\"C1WSDV-BRUSS\" " +
      //  "ClientAddr=\"172.18.172.46\" " +
      //  "EstimateOnly=\"true\" " +
      //  "RedirectToBasket=\"false\" " +
      //  "EnteredBy=\"customer\" " +
      //  "ExpectedTotal=\"-1\" " +
      //  "FromApp=\"Atlantis.Framework.BonsaiPurchase.Tests\" " +
      //  "OrderSource=\"Online\" " +
      //  "SendConfirmEmail=\"true\" " +
      //  "TransactionCurrency=\"USD\">" +
      //  "</ClientChange>";

      //string itemRequestXml =
      //  "<itemRequest isc=\"muiIscDev\" addClientIP=\"172.18.172.46\"><itemTemplate itemTrackingCode=\"mui_hosting\"   /><item unifiedProductID=\"4601\" quantity=\"1\" duration=\"2\" itemTrackingCode=\"mui_hosting\" /></itemRequest>";

      //var request = new BonsaiPurchaseRequestData("80420", "", "", "", 0, "433883", "hosting", "billing", purchaseXml,
      //                                            0, 0, itemRequestXml);

      //var response = (BonsaiPurchaseResponseData)Engine.Engine.ProcessRequest(request, 630);

      //Debug.WriteLine(response.OrderXml);

      //Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.BonsaiPurchase.Impl.dll")]
    public void BonsaiPurchaseHostingRenewalDedIpAddOnPrepaid()
    {
      // Note: you will need to change the purchaseXml after each test with EstimateOnly=false
      string purchaseXml =
        "<ClientChange TreeID=\"1294\" ShopperID=\"840420\" " +
        "RequestingApp=\"Atlantis.Framework.BonsaiPurchase.Tests\" " +
        "RequestingAppHost=\"C1WSDV-BRUSS\" " +
        "ClientAddr=\"172.18.172.46\" " +
        "EstimateOnly=\"true\" " +
        "RedirectToBasket=\"false\" " +
        "EnteredBy=\"customer\" " +
        "ExpectedTotal=\"-1\" " +
        "FromApp=\"Atlantis.Framework.BonsaiPurchase.Tests\" " +
        "OrderSource=\"Online\" " +
        "SendConfirmEmail=\"true\" " +
        "TransactionCurrency=\"USD\">" +
        "<Branches><Branch BranchID=\"9\"> " +
        "<SelectedNodes>" +
        "<SelectedNode SelectedNodeID=\"25:26\" " +
        "Qty=\"10\" StartDate=\"\" EndDate=\"\" />" +
        "</SelectedNodes>" +
        "</Branch></Branches>" +
        //"<Prepaid><PrepaidItem UnifiedProductID=\"69\" " +
        //"Quantity=\"10\" StartMonth=\"12\" StartYear=\"2012\" " +
        //"EndMonth=\"6\" EndYear=\"2013\" /></Prepaid>" +
        "</ClientChange>";

      string itemRequestXml =
        "<itemRequest isc=\"muiIscDev\" addClientIP=\"172.18.172.46\"><itemTemplate itemTrackingCode=\"mui_hosting\"   /></itemRequest>";
      //string itemRequestXml =
      //  "<itemRequest isc=\"muiIscDev\" addClientIP=\"172.18.172.46\"><itemTemplate itemTrackingCode=\"mui_hosting\"   /><item unifiedProductID=\"4601\" quantity=\"1\" duration=\"2\" itemTrackingCode=\"mui_hosting\" /></itemRequest>";

      var request = new BonsaiPurchaseRequestData("80420", "", "", "", 0, "433883", "hosting", "billing", purchaseXml,
                                                  17601, 6, itemRequestXml);

      var response = (BonsaiPurchaseResponseData)Engine.Engine.ProcessRequest(request, 630);

      Debug.WriteLine(response.OrderXml);

      Assert.IsTrue(response.IsSuccess);
    }

  }
}
