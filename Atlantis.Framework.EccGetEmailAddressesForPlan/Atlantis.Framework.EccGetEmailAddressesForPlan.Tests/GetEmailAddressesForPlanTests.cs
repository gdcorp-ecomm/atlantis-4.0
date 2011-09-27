using System;
using System.Diagnostics;
using Atlantis.Framework.EccGetEmailAddressesForPlan.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EccGetEmailAddressesForPlan.Tests
{
  [TestClass]
  public class GetEmailAddressesForPlanTests
  {
    [TestMethod]
    [DeploymentItem("app.config")]
    public void EccGetEmailAddressInfo_RequestData()
    {
      string shopperId = "858421"; //"85408"; // //"858965"; 

      int requestType = 234;
      TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);
      string accountUid = "12b6982d-48c6-11df-b65b-005056956427";

      RequestData requestData = new EccGetEmailAddressesForPlanRequestData(shopperId,
                                                                                "http://localhost",
                                                                                 Int32.MinValue.ToString(),
                                                                                "localhost",
                                                                                1,
                                                                                1,
                                                                                accountUid,
                                                                                false
                                                                                );

     

      try
      {
        var setEmailAccountResponseData = (EccGetEmailAddressesForPlanResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
        Debug.WriteLine(setEmailAccountResponseData.ToXML());
        Assert.IsTrue(setEmailAccountResponseData.IsSuccess);
        Debug.WriteLine("count: " + setEmailAccountResponseData.Results.Count);

      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
