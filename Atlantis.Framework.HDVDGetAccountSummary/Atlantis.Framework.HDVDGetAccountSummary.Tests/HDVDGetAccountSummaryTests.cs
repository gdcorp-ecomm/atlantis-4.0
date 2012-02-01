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
      Guid theguid = new Guid("f48be517-e0ab-45f7-8766-ad761f241f5d");
      //Guid theguid = new Guid("99a77cac-c7f2-11de-8ec2-005056952fd6");
      string _shopperId = "12530";
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

      HDVDGetAccountSummaryResponseData response = Engine.Engine.ProcessRequest(request, requestId) as HDVDGetAccountSummaryResponseData;
      Assert.IsFalse(response.IsSuccess);
      Debug.WriteLine(response.ToXML());
      Debug.WriteLine(response.StatusMessage);

    }

  } 
}
