using System;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCAddPackage.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCAddPackage.Tests
{
  [TestClass]
  public class QSCAddPackageTests
  {
    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCAddPackage.Impl.dll")]
    public void AddPackageWithOneItemToOrderIsSuccessful()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2222";
      int requestId = 558;

      // add a itemReference object with qty:1 and itemId:1582
      itemReference itemToAdd = new itemReference();
      itemToAdd.itemIdRef = 1582;
      itemToAdd.quantity = 1;

      QSCAddPackageRequestData request = new QSCAddPackageRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId);

      // add the item to the request
      request.Items.Add(itemToAdd);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCAddPackageResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCAddPackageResponseData;

      Assert.IsTrue(response.IsSuccess);

      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCAddPackage.Impl.dll")]
    public void AddPackageWithItemNotOnOrderFails()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2222";
      int requestId = 558;

      // add a itemReference object with qty:1 and itemId:1580
      itemReference itemToAdd = new itemReference();
      itemToAdd.itemIdRef = 1580;
      itemToAdd.quantity = 1;

      QSCAddPackageRequestData request = new QSCAddPackageRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId);

      // add the item to the request
      request.Items.Add(itemToAdd);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCAddPackageResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCAddPackageResponseData;

      Assert.IsFalse(response.IsSuccess);

      Console.WriteLine(response.ToXML());
    }
  }
}
