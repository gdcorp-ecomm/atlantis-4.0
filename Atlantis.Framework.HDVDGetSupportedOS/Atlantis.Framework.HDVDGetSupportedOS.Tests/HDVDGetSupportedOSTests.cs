using System;
using System.Diagnostics;
using Atlantis.Framework.HDVDGetSupportedOS.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDGetSupportedOS.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class HDVDGetSupportedOSTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetSupportedOperatingSystems()
    {

      //Guid theguid = new Guid("ad10814e-b345-4a30-9871-46dca4e61d3a");
      //Guid theguid = new Guid("ad10814e-b345-4a30-9871-46dca4e61d3a");
      Guid theguid = new Guid("d11319d0-4d10-11e1-83a0-0050569575d8");
      //Guid theguid = new Guid("99a77cac-c7f2-11de-8ec2-005056952fd6");
      string _shopperId = "858421";
      int requestId = 401;
      HDVDGetSupportedOSRequestData request = new HDVDGetSupportedOSRequestData(
        _shopperId,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        theguid
      );

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDGetSupportedOSResponseData response = Engine.Engine.ProcessRequest(request, requestId) as HDVDGetSupportedOSResponseData;
      if (response != null)
      {
        Assert.IsTrue(response.IsSuccess);
        Debug.WriteLine(response.ToXML());
      }
      else
      {
        Assert.Fail("Null Response Recieved!");
      }
    }

  }
}
