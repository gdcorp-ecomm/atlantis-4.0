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
      string _shopperId = "12530";

      //Guid accountUid = new Guid("ad10814e-b345-4a30-9871-46dca4e61d3a");
      Guid accountUid = new Guid("f48be517-e0ab-45f7-8766-ad761f241f5d");
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

        Assert.IsTrue(request.AccountUid == new Guid("f48be517-e0ab-45f7-8766-ad761f241f5d"));
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
      string _shopperId = "12530";

      //Guid accountUid = new Guid("ad10814e-b345-4a30-9871-46dca4e61d3a");
      Guid accountUid = new Guid("f48be517-e0ab-45f7-8766-ad761f241f5d");
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

        Assert.IsTrue(request.AccountUid == new Guid("f48be517-e0ab-45f7-8766-ad761f241f5d"));
        Assert.IsTrue(request.RequestTimeout == TimeSpan.FromSeconds(30));

        var response = Engine.Engine.ProcessRequest(request, 999) as HDVDRequestAddIpResponseData;

        Assert.IsNotNull(response);
        Assert.IsInstanceOfType(response, typeof(HDVDRequestAddIpResponseData));
        Assert.IsNotNull(response.Result);
        Assert.IsInstanceOfType(response.Result, typeof(IHDVDHostingResponse));

        Assert.IsNotNull(response.ToXML());
        Debug.WriteLine(response.ToXML());

        Assert.IsTrue(response.IsSuccess);

     
    }
  }
}
