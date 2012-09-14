using System;
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
		[DeploymentItem("Atlantis.Framework.QSCGetAccounts.Impl.dll")]
		public void GetAccountListWithShopperIdThatDoesNotHaveQsc()
    {
      string _shopperId = "847235";
      int requestId = 541;

      QSCGetAccountsRequestData request = new QSCGetAccountsRequestData(_shopperId, "", string.Empty, string.Empty, 1);

      request.RequestTimeout = TimeSpan.FromSeconds(32);

      QSCGetAccountsResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetAccountsResponseData;

      Assert.IsTrue(response.Response.responseStatus.statusCode == statusCode.SUCCESS_WITH_WARNINGS);

      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCGetAccounts.Impl.dll")]
    public void GetAccountListWithShopperIdThatHasQsc()
    {
      string _shopperId = "859775";
      int requestId = 541;

      QSCGetAccountsRequestData request = new QSCGetAccountsRequestData(_shopperId, "", string.Empty, string.Empty, 1);

      request.RequestTimeout = TimeSpan.FromSeconds(32);

      QSCGetAccountsResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetAccountsResponseData;

      Assert.IsTrue(response.AccountList.Count >= 1);

      Console.WriteLine(response.ToXML());
    }
  }
}
