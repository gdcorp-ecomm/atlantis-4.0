using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RemoveBulkdomain.Interface;
using Atlantis.Framework.RemoveBulkdomain.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RemoveBulkDomain.Test
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.RemoveBulkDomain.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.RemoveBulkDomain.Interface.dll")]
  public class UnitTest1
  {
    //private int _shopperId = 126599;
    //private int _orderId = 543074;

    [TestMethod]
    public void TestMethod1()
    {
      //string secDomainName = "24MONTHSOFWSTGOODNESS";
      //string topLevelDomain = "FIRM.IN";
      //int rowID = 15;
      //var request = new RemoveBulkDomainRequestData(_shopperId.ToString(), string.Empty, _orderId.ToString(), string.Empty, 0, secDomainName, topLevelDomain, rowID);
      //var response = Engine.Engine.ProcessRequest(request, 105) as RemoveBulkDomainResponseData;

      // This unit test is here to facilitate testing a shopper that exists in test.
      // If used, make sure to set the _shopperId, _orderId, secDomainName, topLevelDomain, and rowID

      Assert.IsTrue(true);
    }
  }
}
