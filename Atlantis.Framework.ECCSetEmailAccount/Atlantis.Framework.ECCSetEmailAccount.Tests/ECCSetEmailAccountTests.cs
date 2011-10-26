using System;
using System.Diagnostics;
using Atlantis.Framework.ECCSetEmailAccount.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCSetEmailAccount.Tests
{
  [TestClass]
  public class ECCSetEmailAccountTests
  {
    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //public void CartECCSetEmailPlansTest()
    //{
    //  string shopperId = "75866"; //"85408"; // //"858965"; 

    //  int requestType = 231;
    //  TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);

    //  RequestData requestData = new EccSetEmailAccountRequestData(shopperId,
    //                                                                            "http://localhost",
    //                                                                             Int32.MinValue.ToString(),
    //                                                                            "localhost",
    //                                                                            1
    //                                                                            );


    //  ((EccSetEmailAccountRequestData)requestData).AccountUid = "2c1f12d7-e860-11df-8798-005056952fd6";
    //  ((EccSetEmailAccountRequestData)requestData).EmailAddress = "theron@123412341234123FFDFEFE.CO";
    //  ((EccSetEmailAccountRequestData)requestData).Password = "Hello";
    //  ((EccSetEmailAccountRequestData)requestData).RequestTimeout = new TimeSpan(0, 0, 0, 10);
    //  ((EccSetEmailAccountRequestData)requestData).ResellerId = 1;
    //  ((EccSetEmailAccountRequestData)requestData).DiskSpace = 250;

    //  try
    //  {
    //    var setEmailAccountResponseData = (EccSetEmailAccountResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
    //    Debug.WriteLine(setEmailAccountResponseData.ToXML());
    //    Assert.IsTrue(setEmailAccountResponseData.IsSuccess);

    //  }
    //  catch (Exception ex)
    //  {
    //    Assert.Fail(ex.Message);
    //  }
    //}

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EccSetEmailPlansForShopperTest()
    {
      string shopperId = "858421"; //"85408"; // //"858965"; 

      int requestType = 231;
      TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);

      RequestData requestData = new EccSetEmailAccountRequestData(shopperId,
                                                                                "http://localhost",
                                                                                 Int32.MinValue.ToString(),
                                                                                "localhost",
                                                                                1
                                                                                );


      ((EccSetEmailAccountRequestData)requestData).AccountUid = "12b6982d-48c6-11df-b65b-005056956427";
      ((EccSetEmailAccountRequestData)requestData).CCList = "kklink@godaddy.com, trwalker@godaddy.com";
      ((EccSetEmailAccountRequestData)requestData).EmailAddress = "distro@asdfasdfasdf.com";
      ((EccSetEmailAccountRequestData)requestData).Password = "T3st123!";
      ((EccSetEmailAccountRequestData)requestData).RequestTimeout = requestTimeout;
      ((EccSetEmailAccountRequestData)requestData).ResellerId = 1;
      ((EccSetEmailAccountRequestData)requestData).DiskSpace = 250;

      try
      {
        var setEmailAccountResponseData = (EccSetEmailAccountResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
        Debug.WriteLine(setEmailAccountResponseData.ToXML());
        Assert.IsTrue(setEmailAccountResponseData.IsSuccess);

      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }


    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //public void EccSetEmailPlansForShopperTest_Forward()
    //{
    //  string shopperId = "858421"; //"85408"; // //"858965"; 

    //  int requestType = 231;
    //  TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);

    //  RequestData requestData = new EccSetEmailAccountRequestData(shopperId,
    //                                                                            "http://localhost",
    //                                                                             Int32.MinValue.ToString(),
    //                                                                            "localhost",
    //                                                                            1
    //                                                                            );


    //  ((EccSetEmailAccountRequestData)requestData).AccountUid = "12b6982d-48c6-11df-b65b-005056956427";
    //  ((EccSetEmailAccountRequestData)requestData).AutoResponderFrom = string.Empty;
    //  ((EccSetEmailAccountRequestData)requestData).AutoResponderMessage = string.Empty;
    //  ((EccSetEmailAccountRequestData)requestData).AutoResponderStatus = "-1";
    //  ((EccSetEmailAccountRequestData)requestData).AutoResponderSubject = string.Empty;
    //  ((EccSetEmailAccountRequestData)requestData).AutoResponsderEnd = DateTime.MinValue;
    //  ((EccSetEmailAccountRequestData)requestData).AutoResponsderStart = DateTime.MinValue;
    //  ((EccSetEmailAccountRequestData)requestData).CCList = string.Empty;
    //  ((EccSetEmailAccountRequestData)requestData).DiskSpace = 120;
    //  ((EccSetEmailAccountRequestData)requestData).EmailAddress = "kklink2@asdfasdfasdf.com";
    //  ((EccSetEmailAccountRequestData)requestData).HasSpamFilter = false;
    //  ((EccSetEmailAccountRequestData)requestData).IsCatchAll = true;
    //  ((EccSetEmailAccountRequestData)requestData).Password = "123";
    //  ((EccSetEmailAccountRequestData)requestData).RequestTimeout = new TimeSpan(0, 0, 0, 10);
    //  ((EccSetEmailAccountRequestData)requestData).ResellerId = 1;
    //  ((EccSetEmailAccountRequestData)requestData).SendSingleResponse = false;
    //  ((EccSetEmailAccountRequestData)requestData).SmtpRelays = 250;
    //  ((EccSetEmailAccountRequestData)requestData).Subaccount = "";

    //  try
    //  {
    //    var setEmailAccountResponseData = (EccSetEmailAccountResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
    //    Debug.WriteLine(setEmailAccountResponseData.ToXML());
    //    Assert.IsTrue(setEmailAccountResponseData.IsSuccess);

    //  }
    //  catch (Exception ex)
    //  {
    //    Assert.Fail(ex.Message);
    //  }
    //}

  }
}
