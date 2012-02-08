using System;
using System.Diagnostics;
using Atlantis.Framework.HDVD.Interface.Interfaces;
using Atlantis.Framework.HDVDRequestAddIP.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDRequestAddIP.Tests
{
  [TestClass]
  public class HDVDRequestAddIPTests
  {
    [TestMethod]
    public void CreateValidRequest()
    {
      string _shopperId = "858421";

      //Guid accountUid = new Guid("ad10814e-b345-4a30-9871-46dca4e61d3a");
      Guid accountUid = new Guid("d11319d0-4d10-11e1-83a0-0050569575d8");
      //Guid accountUid = new Guid("99a77cac-c7f2-11de-8ec2-005056952fd6");

      try
      {
        HDVDRequestAddIpRequestData request = new HDVDRequestAddIpRequestData(
          _shopperId,
          string.Empty,
          string.Empty,
          string.Empty,
          1,
          accountUid);

        request.RequestTimeout = TimeSpan.FromSeconds(30);

        Assert.IsInstanceOfType(request, typeof(HDVDRequestAddIpRequestData));

        Assert.IsTrue(request.AccountUid == new Guid("d11319d0-4d10-11e1-83a0-0050569575d8"));
        Assert.IsTrue(request.RequestTimeout == TimeSpan.FromSeconds(30));
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }

    }

    [TestMethod]
    public void CreateValidRequestAndExecute()
    {
      string _shopperId = "858421";

      //Guid accountUid = new Guid("ad10814e-b345-4a30-9871-46dca4e61d3a");
      Guid accountUid = new Guid("d11319d0-4d10-11e1-83a0-0050569575d8");
      //Guid accountUid = new Guid("99a77cac-c7f2-11de-8ec2-005056952fd6");

     
        HDVDRequestAddIpRequestData request = new HDVDRequestAddIpRequestData(
          _shopperId,
          string.Empty,
          string.Empty,
          string.Empty,
          1,
          accountUid);

        request.RequestTimeout = TimeSpan.FromSeconds(30);

        Assert.IsInstanceOfType(request, typeof(HDVDRequestAddIpRequestData));

        Assert.IsTrue(request.AccountUid == new Guid("d11319d0-4d10-11e1-83a0-0050569575d8"));
        Assert.IsTrue(request.RequestTimeout == TimeSpan.FromSeconds(30));

        var response = Engine.Engine.ProcessRequest(request, 999) as HDVDRequestAddIpResponseData;

        Assert.IsNotNull(response);
        Assert.IsInstanceOfType(response, typeof(HDVDRequestAddIpResponseData));
        Assert.IsNotNull(response.Response);
        
        Assert.IsNotNull(response.ToXML());
        Debug.WriteLine(response.ToXML());

        Assert.IsTrue(response.IsSuccess);

     
    }
  }
}
