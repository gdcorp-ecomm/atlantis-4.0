using System;
using System.Diagnostics;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.ECCGetSmtpRelayInfo.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetSmtpRelayInfo.Tests
{
  [TestClass]
  public class ECCGetSmtpRelayInfoTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EccGetEmailPlansForShopperTest()
    {
      string shopperId = "85408"; //"858421"; //"858965";
     
      int requestType = 229;
      TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);

      RequestData requestData = new ECCGetSmtpRelayInfoRequestData(shopperId,
                                                                                "http://localhost",
                                                                                 Int32.MinValue.ToString(),
                                                                                "localhost",
                                                                                1,
                                                                                1,
                                                                                EmailTypes.All,
                                                                                requestTimeout
                                                                                );




      try
      {
        var getEmailPlansForShopperResponseData = (ECCGetSmtpRelayInfoResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
        Debug.WriteLine(getEmailPlansForShopperResponseData.ToXML());
        Assert.IsTrue(getEmailPlansForShopperResponseData.IsSuccess);

      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

  }
}
