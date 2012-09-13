using System;
using System.Collections.Generic;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCUpdatePackage.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCUpdatePackage.Tests
{
  [TestClass]
  public class QSCUpdatePackageTests
  {
    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCUpdatePackage.Impl.dll")]
		public void AddPackageWithOneItemToOrderIsSuccessful()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2222";
      int requestId = 559;

      package _updatePackage = new package();

      _updatePackage.packageId = 4118;
      _updatePackage.trackingNumber = string.Empty;
      _updatePackage.shipDate = DateTime.Now.ToShortDateString();

      List<itemReference> items = new List<itemReference>(1);

      // add a itemReference object with qty:2 and itemId:1582
      itemReference itemToAdd = new itemReference {itemIdRef = 1582, quantity = 2};
      items.Add(itemToAdd);
      _updatePackage.itemPackingRef = items.ToArray();

      money packageCost = new money {currencyCode = "USD", value = 3.95m};
      _updatePackage.packageCost = packageCost;

      QSCUpdatePackageRequestData request = new QSCUpdatePackageRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _updatePackage);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCUpdatePackageResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCUpdatePackageResponseData;

      Assert.IsTrue(response.IsSuccess);

      Console.WriteLine(response.ToXML());
    }
  }
}
