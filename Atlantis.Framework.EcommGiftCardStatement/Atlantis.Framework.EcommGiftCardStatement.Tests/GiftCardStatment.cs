using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.EcommGiftCardStatement.Interface;

namespace Atlantis.Framework.EcommGiftCardStatement.Tests
{
  [TestClass]
  public class GiftCardStatment
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.EcommGiftCardStatement.Impl.dll")]
    public void TestMethod1()
    {
      EcommGiftCardStatementRequestData request = new EcommGiftCardStatementRequestData("856907", string.Empty, string.Empty, string.Empty, 0, 1030484);
      EcommGiftCardStatementResponseData response = (EcommGiftCardStatementResponseData)Engine.Engine.ProcessRequest(request, 720);

      List<GiftCardTransaction> gcList = new List<GiftCardTransaction>();
      gcList = response.GiftCardTransactionList;
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
