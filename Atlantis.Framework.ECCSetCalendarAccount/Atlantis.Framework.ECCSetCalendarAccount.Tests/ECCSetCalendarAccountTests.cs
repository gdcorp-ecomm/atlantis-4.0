using System;
using Atlantis.Framework.ECCSetCalendarAccount.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCSetCalendarAccount.Tests
{
  [TestClass]
  public class ECCSetCalendarAccountTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void SetCalendarAccountForShopperEmail()
    {
      Guid testGuid = new Guid("048b714c-dd2e-11df-a9c4-0050569575d8");
      
      ECCSetCalendarAccountRequestData requestData = new ECCSetCalendarAccountRequestData("859775",
        1,
        "cal5@bitbucket.com",
        testGuid,
        "Calendar activated by mobile!",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCSetCalendarAccountResponseData responseData = (ECCSetCalendarAccountResponseData)Engine.Engine.ProcessRequest(requestData, 258);

        if(!responseData.IsSuccess)
        {
          Console.WriteLine("Result Code: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer.ToString());
          Console.WriteLine("Message: {0}",responseData.Response.Item.Message);
          Assert.Fail("Call was not successful. If this test failed you may need to change the email address.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.ResultCode == 0);

          Console.WriteLine("Result Code: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer.ToString());
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
    public void SetDuplicateCalendarAccountForShopperEmail()
    {
      Guid testGuid = new Guid("048b714c-dd2e-11df-a9c4-0050569575d8");

      ECCSetCalendarAccountRequestData requestData = new ECCSetCalendarAccountRequestData("859775",
        1,
        "cal0@bitbucket.com",
        testGuid,
        "Calendar activated by mobile!",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCSetCalendarAccountResponseData responseData = (ECCSetCalendarAccountResponseData)Engine.Engine.ProcessRequest(requestData, 258);

        if (responseData.IsSuccess)
        {
          Assert.Fail("There was an error and the ECC API allowed a duplicate calendar to be set.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.ResultCode == 35004);

          Console.WriteLine("Result Code: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
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
    public void SetCalendarAccountForInvalidShopperId()
    {
      Guid testGuid = new Guid("048b714c-dd2e-11df-a9c4-0050569575d8");

      ECCSetCalendarAccountRequestData requestData = new ECCSetCalendarAccountRequestData("ASDFASDF",
        1,
        "cal9@bitbucket.com",
        testGuid,
        "Calendar activated by mobile!",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCSetCalendarAccountResponseData responseData = (ECCSetCalendarAccountResponseData)Engine.Engine.ProcessRequest(requestData, 258);

        if (responseData.IsSuccess)
        {
          Assert.Fail("There was an error and the ECC API allowed a calendar to be set for an invalid Shopper Id.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.ResultCode != 0);

          Console.WriteLine("Result Code: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
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
    public void SetCalendarAccountForShopperWithInvalidEmail()
    {
      Guid testGuid = new Guid("048b714c-dd2e-11df-a9c4-0050569575d8");

      ECCSetCalendarAccountRequestData requestData = new ECCSetCalendarAccountRequestData("859775",
        1,
        "nosuchmail@bitbucket.com",
        testGuid,
        "Calendar activated by mobile!",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCSetCalendarAccountResponseData responseData = (ECCSetCalendarAccountResponseData)Engine.Engine.ProcessRequest(requestData, 258);

        if (responseData.IsSuccess)
        {
          Assert.Fail("There was an error and the ECC API allowed a calendar to be set on an invalid email.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.ResultCode != 0 && responseData.Response.Item.ResultCode != 35004);

          Console.WriteLine("Result Code: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
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
    public void SetCalendarAccountForShopperWithInvalidGuid()
    {
      Guid testGuid = new Guid("00000000-dd2e-11df-a9c4-0050569575d8");

      ECCSetCalendarAccountRequestData requestData = new ECCSetCalendarAccountRequestData("859775",
        1,
        "cal8@bitbucket.com",
        testGuid,
        "Calendar activated by mobile!",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCSetCalendarAccountResponseData responseData = (ECCSetCalendarAccountResponseData)Engine.Engine.ProcessRequest(requestData, 258);

        if (responseData.IsSuccess)
        {
          Assert.Fail("There was an error and the ECC API allowed a calendar to be set on an invalid email plan GUID.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.ResultCode != 0);

          Console.WriteLine("Result Code: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
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
    public void SetCalendarAccountForShopperOnCalendarPlanWithNoRoom()
    {
      Guid testGuid = new Guid("048b714a-dd2e-11df-a9c4-0050569575d8");

      ECCSetCalendarAccountRequestData requestData = new ECCSetCalendarAccountRequestData("859775",
        1,
        "cal7@bitbucket.com",
        testGuid,
        "Calendar activated by mobile!",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCSetCalendarAccountResponseData responseData = (ECCSetCalendarAccountResponseData)Engine.Engine.ProcessRequest(requestData, 258);

        if (responseData.IsSuccess)
        {
          Assert.Fail("There was an error and the ECC API allowed a calendar to be set using a plan with no remaining users.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.ResultCode != 0 && responseData.Response.Item.ResultCode != 35004);

          Console.WriteLine("Result Code: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
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
    public void SetCalendarAccountForShopperWithMissingDomain()
    {
      Guid testGuid = new Guid("048b714c-dd2e-11df-a9c4-0050569575d8");

      ECCSetCalendarAccountRequestData requestData = new ECCSetCalendarAccountRequestData("859775",
        1,
        "test",
        testGuid,
        "Calendar activated by mobile!",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCSetCalendarAccountResponseData responseData = (ECCSetCalendarAccountResponseData)Engine.Engine.ProcessRequest(requestData, 258);

        if (responseData.IsSuccess)
        {
          Assert.Fail("There was an error and the ECC API allowed an invalid email to be used.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.ResultCode == 230);

          Console.WriteLine("Result Code: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
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
    public void SetCalendarAccountForShopperWithMissingTLD()
    {
      Guid testGuid = new Guid("048b714c-dd2e-11df-a9c4-0050569575d8");

      ECCSetCalendarAccountRequestData requestData = new ECCSetCalendarAccountRequestData("859775",
        1,
        "test@bitbucket",
        testGuid,
        "Calendar activated by mobile!",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCSetCalendarAccountResponseData responseData = (ECCSetCalendarAccountResponseData)Engine.Engine.ProcessRequest(requestData, 258);

        if (responseData.IsSuccess)
        {
          Assert.Fail("There was an error and the ECC API allowed an invalid email to be used.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.ResultCode == 233);

          Console.WriteLine("Result Code: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
