using System;
using System.Diagnostics;
using Atlantis.Framework.HDVDGetAccountSummary.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDGetAccountSummary.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class HDVDGetAccountSummaryTests
  {

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetAccountSummary_PendingSetup()
    {
      const string APPID = "HDVDACCOUNTSUMMARY_UNITTEST";
      //Guid theguid = new Guid("ad10814e-b345-4a30-9871-46dca4e61d3a");
      //Guid theguid = new Guid("d11319d0-4d10-11e1-83a0-0050569575d8");
      Guid theguid = new Guid("87dcb01a-79d0-11e1-a38b-0050569575d8");
      string _shopperId = "858421";
      int requestId = 399;
      HDVDGetAccountSummaryRequestData request = new HDVDGetAccountSummaryRequestData(
        _shopperId,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        APPID,
        theguid
      );

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDGetAccountSummaryResponseData response = (HDVDGetAccountSummaryResponseData)Engine.Engine.ProcessRequest(request, requestId);
      Assert.IsTrue(response.IsSuccess);
      Debug.WriteLine(response.ToXML());
      Debug.WriteLine(response.Response.Message);

    }

  } 
}
