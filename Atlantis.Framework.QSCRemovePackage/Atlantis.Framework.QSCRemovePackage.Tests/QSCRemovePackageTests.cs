using System;
using Atlantis.Framework.QSCRemovePackage.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCRemovePackage.Tests
{
  [TestClass]
  public class QSCRemovePackageTests
  {
    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCRemovePackage.Impl.dll")]
		public void RemoveInvalidPackageIdFromOrderFails()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2222";
      int requestId = 560;
      int packageIdToRemove = 4119;
      
      QSCRemovePackageRequestData request = new QSCRemovePackageRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, packageIdToRemove);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCRemovePackageResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCRemovePackageResponseData;

      Assert.IsFalse(response.IsSuccess);

      Console.WriteLine(response.ToXML());
    }

    // This test must have a valid EXISTING package id specified below in order to pass...
    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCRemovePackage.Impl.dll")]
		public void RemoveValidPackageIdFromOrderPasses()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2222";
      int requestId = 560;

      // Update this item to reflect an EXISTING package you want removed from the order
      int packageIdToRemove = 4119;

      QSCRemovePackageRequestData request = new QSCRemovePackageRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, packageIdToRemove);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCRemovePackageResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCRemovePackageResponseData;

      Assert.IsTrue(response.IsSuccess);

      Console.WriteLine(response.ToXML());
    }
  }
}
