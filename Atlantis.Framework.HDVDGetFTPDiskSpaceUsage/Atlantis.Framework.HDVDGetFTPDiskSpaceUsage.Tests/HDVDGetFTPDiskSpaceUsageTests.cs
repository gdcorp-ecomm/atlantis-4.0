using System;
using Atlantis.Framework.HDVDGetFTPDiskSpaceUsage.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDGetFTPDiskSpaceUsage.Tests
{
  [TestClass]
  public class HDVDGetFTPDiskSpaceUsageTests
  {
    private const string _SHOPPER_ID = "858421";
    private const int _REQUEST_ID = 499;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void NeagtiveTestWithUnsupportedAccountGuid()
    {
      const string accountGuid = "b3477647-2161-11df-8913-005056956427";

      HDVDGetFTPDiskSpaceUsageRequestData request = new HDVDGetFTPDiskSpaceUsageRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDGetFTPDiskSpaceUsageResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDGetFTPDiskSpaceUsageResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsTrue(response.Status == "error");
      Assert.IsTrue(response.Message.Equals("Product type is not supported"));
      Assert.IsTrue(response.StatusCode == 7);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PositiveTest()
    {
      const string accountGuid = "d11319d0-4d10-11e1-83a0-0050569575d8";

      HDVDGetFTPDiskSpaceUsageRequestData request = new HDVDGetFTPDiskSpaceUsageRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDGetFTPDiskSpaceUsageResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDGetFTPDiskSpaceUsageResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsTrue(response.Status == "success");
      Assert.IsTrue(response.Message.Equals("success"));
      Assert.IsTrue(response.StatusCode == 0);
      Assert.IsTrue(response.CurrentMonthUsage != null);
      Assert.IsTrue(response.PreviousMonthUsage != null);
    }
  }
}
