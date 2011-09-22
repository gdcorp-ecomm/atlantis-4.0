using System;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.ECCGetCalendarPlans.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetCalendarPlans.Tests
{
  [TestClass]
  public class ECCGetCalendarPlansTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetAllCalendarPlansForShopper()
    {
      ECCGetCalendarPlansRequestData requestData = new ECCGetCalendarPlansRequestData("859775",
                                                                                      1,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.
                                                                                      Empty,
                                                                                      0);

      try
      {
        ECCGetCalendarPlansResponseData responseData = (ECCGetCalendarPlansResponseData)Engine.Engine.ProcessRequest(requestData, 272);

        if(!responseData.IsSuccess)
        {
          Console.WriteLine("ResultCode: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Assert.Fail("Call was not successful.");
        }
        else
        {
          Assert.IsTrue(responseData.IsSuccess);
          Console.WriteLine("ResultCode: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Assert.IsTrue(responseData.Response.Item.Results.Count > 0);
          Console.WriteLine("Items returned: {0}",responseData.Response.Item.Results.Count);
          Console.WriteLine(responseData.ToXML());
        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetSpecificCalendarPlanForShopper()
    {
      ECCGetCalendarPlansRequestData requestData = new ECCGetCalendarPlansRequestData("859775",
                                                                                      1,
                                                                                      "048b714c-dd2e-11df-a9c4-0050569575d8",
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.
                                                                                      Empty,
                                                                                      0);

      try
      {
        ECCGetCalendarPlansResponseData responseData = (ECCGetCalendarPlansResponseData)Engine.Engine.ProcessRequest(requestData, 272);

        if (!responseData.IsSuccess)
        {
          Console.WriteLine("ResultCode: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Assert.Fail("Call was not successful.");
        }
        else
        {
          Assert.IsTrue(responseData.IsSuccess);
          Console.WriteLine("ResultCode: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Assert.IsTrue(responseData.Response.Item.Results.Count > 0);
          Console.WriteLine("Items returned: {0}", responseData.Response.Item.Results.Count);
          Assert.IsTrue(responseData.Response.Item.Results[0].ProductType == EccAccountType.GroupCalendarPack);
          Assert.IsTrue(responseData.Response.Item.Results[0].Pfid == 835);

          //Console.WriteLine("AccountUid: {0}",responseData.Response.Item.Results[0].AccountGuid.ToString());
          Console.WriteLine("ProductName: {0}", Convert.ToString(responseData.Response.Item.Results[0].ProductType));
          Console.WriteLine("Status: {0}", responseData.Response.Item.Results[0].Status);
          Console.WriteLine("ExpireDate: {0}", responseData.Response.Item.Results[0].ExpirationDate.ToString("yyyy-MM-dd"));
          Console.WriteLine("addl_users_num: {0}", responseData.Response.Item.Results[0].UsersAdditional);
          Console.WriteLine("group_calendar_allowed_num: {0}", responseData.Response.Item.Results[0].UsersAllowed);
          Console.WriteLine("pf_id: {0}", responseData.Response.Item.Results[0].Pfid);
          Console.WriteLine("remaining_users: {0}", responseData.Response.Item.Results[0].UsersRemaining);
          Console.WriteLine(responseData.ToXML());
        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetCalendarPlansForInvalidShopper()
    {
      ECCGetCalendarPlansRequestData requestData = new ECCGetCalendarPlansRequestData("ASDFASDF",
                                                                                      1,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.
                                                                                      Empty,
                                                                                      0);

      try
      {
        ECCGetCalendarPlansResponseData responseData = (ECCGetCalendarPlansResponseData)Engine.Engine.ProcessRequest(requestData, 272);

        if (responseData.IsSuccess)
        {
          Console.WriteLine("ResultCode: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Assert.Fail("ECC Service returned a false positive. Ecc API should return an error if a bad Shopper Id is presented.");
        }
        else
        {
          Assert.IsFalse(responseData.Response.Item.ResultCode == 0);
          
          Console.WriteLine("ResultCode: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetCalendarPlanForShopperWithInvalidUid()
    {
      ECCGetCalendarPlansRequestData requestData = new ECCGetCalendarPlansRequestData("859775",
                                                                                      1,
                                                                                      "00000000-dd2e-11df-a9c4-0050569575d8",
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.
                                                                                      Empty,
                                                                                      0);

      try
      {
        ECCGetCalendarPlansResponseData responseData = (ECCGetCalendarPlansResponseData)Engine.Engine.ProcessRequest(requestData, 272);

        if (responseData.IsSuccess)
        {
          Console.WriteLine("ResultCode: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Assert.Fail("ECC Service returned a false positive. Ecc API should return an error if a bad UID is presented.");
        }
        else
        {
          Assert.IsFalse(responseData.Response.Item.ResultCode == 0);

          Console.WriteLine("ResultCode: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
