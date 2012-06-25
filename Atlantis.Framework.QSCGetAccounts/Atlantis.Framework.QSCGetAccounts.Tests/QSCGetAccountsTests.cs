using System;
using System.Diagnostics;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetAccounts.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCGetAccounts.Tests
{
  [TestClass]
  public class QSCGetAccountsTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetAccountListWithShopperIdThatDoesNotHaveQsc()
    {
      string _shopperId = "859775";
      int requestId = 541;

      QSCGetAccountsRequestData request = new QSCGetAccountsRequestData(_shopperId, "", string.Empty, string.Empty, 1);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCGetAccountsResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetAccountsResponseData;

      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.Response.responseStatus.statusCode == statusCode.SUCCESS_WITH_WARNINGS);

      Debug.WriteLine(response.ToXML());
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetAccountListWithShopperIdThatHasQsc()
    {
      string _shopperId = "837435";
      int requestId = 541;

      QSCGetAccountsRequestData request = new QSCGetAccountsRequestData(_shopperId, "", string.Empty, string.Empty, 1);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCGetAccountsResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetAccountsResponseData;

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.AccountList.Count >= 1);

      Debug.WriteLine(response.ToXML());
    }
  }
}
