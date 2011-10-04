using System;
using System.Diagnostics;
using Atlantis.Framework.EccRenameEmailPlan.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EccRenameEmailPlan.Tests
{
  [TestClass]
  public class EccRenameEmailPlanTests
  {
    [TestMethod]
    public void EccGetEmailAddressInfo_RequestData()
    {
      string shopperId = "858421"; //"85408"; // //"858965"; 

      int requestType = 235;
      TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);
      string accountUid = "12b6982d-48c6-11df-b65b-005056956427";

      RequestData requestData = new EccRenameEmailPlanRequestData(shopperId,
                                                                  "http://localhost",
                                                                    Int32.MinValue.ToString(),
                                                                  "localhost",
                                                                  1,
                                                                  1,
                                                                  string.Empty,
                                                                  accountUid,
                                                                  "Test Email Plan"
                                                                  );

      try
      {
        var renameEmailPlanResponseData = (EccRenameEmailPlanResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
        Debug.WriteLine(renameEmailPlanResponseData.ToXML());
        Assert.IsTrue(renameEmailPlanResponseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
