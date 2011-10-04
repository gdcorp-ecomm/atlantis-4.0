using System;
using System.Diagnostics;
using System.Threading;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.ECCGetEmailPlansForShopper.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetEmailPlansForShopper.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class ECCGetEmailPlansForShopperTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EccGetEmailPlansForShopperTest()
   {
     string shopperId = "858421";
     //string shopperId = "87738";
     int requestType = 225;
     TimeSpan requestTimeout = new TimeSpan(0,0,1,30);

      RequestData requestData = new ECCGetEmailPlansForShopperRequestData(shopperId,
                                                                                "http://localhost",
                                                                                 Int32.MinValue.ToString(),
                                                                                "localhost",
                                                                                1,
                                                                                1,
                                                                                EmailTypes.All,
                                                                                requestTimeout);

     
     

      try
      {
        var getEmailPlansForShopperResponseData = (ECCGetEmailPlansForShopperResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
        Assert.IsTrue(getEmailPlansForShopperResponseData.IsSuccess);
        Debug.WriteLine(getEmailPlansForShopperResponseData.ToXML());
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EccGetEmailPlansForShopperTestFromSessionCache()
    {

      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
   
      string shopperId = "858421";
      //string shopperId = "87738";
      int requestType = 225;
      TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);

      RequestData requestData = new ECCGetEmailPlansForShopperRequestData(shopperId,
                                                                                "http://localhost",
                                                                                 Int32.MinValue.ToString(),
                                                                                "localhost",
                                                                                1,
                                                                                1,
                                                                                EmailTypes.All,
                                                                                requestTimeout);




      try
      {
        var getEmailPlansForShopperResponseData =
          SessionCache.SessionCache.GetProcessRequest<ECCGetEmailPlansForShopperResponseData>(requestData, requestType);
        Assert.IsTrue(getEmailPlansForShopperResponseData.IsSuccess);
        Debug.WriteLine(getEmailPlansForShopperResponseData.ToXML());
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EccGetEmailPlansForShopperTestFromSessionCacheWithTimeout()
    {

      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      string shopperId = "858421";
      //string shopperId = "87738";
      int requestType = 225;
      TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);

      RequestData requestData = new ECCGetEmailPlansForShopperRequestData(shopperId,
                                                                                "http://localhost",
                                                                                 Int32.MinValue.ToString(),
                                                                                "localhost",
                                                                                1,
                                                                                1,
                                                                                EmailTypes.All,
                                                                                requestTimeout);




      try
      {
        var getEmailPlansForShopperResponseData =
          SessionCache.SessionCache.GetProcessRequest<ECCGetEmailPlansForShopperResponseData>(requestData, requestType, new TimeSpan(0,0,2,0));
        Assert.IsTrue(getEmailPlansForShopperResponseData.IsSuccess);
        Debug.WriteLine(getEmailPlansForShopperResponseData.ToXML());
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EccGetEmailPlansForShopperTestFromSessionCacheWithTimeoutAndSecondRequest()
    {

      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      string shopperId = "858421";
      //string shopperId = "87738";
      int requestType = 225;
      TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);

      RequestData requestData = new ECCGetEmailPlansForShopperRequestData(shopperId,
                                                                                "http://localhost",
                                                                                 Int32.MinValue.ToString(),
                                                                                "localhost",
                                                                                1,
                                                                                1,
                                                                                EmailTypes.All,
                                                                                requestTimeout);




      try
      {
        var getEmailPlansForShopperResponseData =
          SessionCache.SessionCache.GetProcessRequest<ECCGetEmailPlansForShopperResponseData>(requestData, requestType, new TimeSpan(0, 0, 2, 0));
        Assert.IsTrue(getEmailPlansForShopperResponseData.IsSuccess);
        Debug.WriteLine(getEmailPlansForShopperResponseData.ToXML());
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }


      Thread.Sleep(1000);

      try
      {
        var getEmailPlansForShopperResponseData =
          SessionCache.SessionCache.GetProcessRequest<ECCGetEmailPlansForShopperResponseData>(requestData, requestType, new TimeSpan(0, 0, 2, 0));
        Assert.IsTrue(getEmailPlansForShopperResponseData.IsSuccess);
        Debug.WriteLine(getEmailPlansForShopperResponseData.ToXML());
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }


    }
  }
}
