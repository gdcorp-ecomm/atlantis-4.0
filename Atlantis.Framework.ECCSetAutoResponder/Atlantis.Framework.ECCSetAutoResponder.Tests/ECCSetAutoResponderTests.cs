using System;
using Atlantis.Framework.ECCSetAutoResponder.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCSetAutoResponder.Tests
{
  [TestClass]
  public class ECCSetAutoResponderTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void SetAutoResponderDetailsForShopperEmail()
    {
      ECCSetAutoResponderRequestData requestData = new ECCSetAutoResponderRequestData("847235",
        1,
        "set@test.com",
        "Set by Unit Test",
        "Unit test subject.",
        1,
        DateTime.Now.AddDays(1),
        DateTime.Now.AddDays(2),
        "unitTest@test.com",
        false,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);
      try
      {
        ECCSetAutoResponderResponseData responseData = (ECCSetAutoResponderResponseData)Engine.Engine.ProcessRequest(requestData, 254);

        if(!responseData.IsSuccess)
        {
          Assert.Fail("Call was not successful.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.ResultCode == 0);

          Console.WriteLine("Result Code: " + responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: " + responseData.Response.Item.Timer.ToString());
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
    public void SetAutoResponderDetailsForInvalidShopper()
    {
      ECCSetAutoResponderRequestData requestData = new ECCSetAutoResponderRequestData("asdfasdf",
        1,
        "set@test.com",
        "Set by Unit Test",
        "Unit test subject.",
        1,
        DateTime.Now.AddDays(1),
        DateTime.Now.AddDays(2),
        "unitTest@test.com",
        false,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);
      try
      {
        ECCSetAutoResponderResponseData responseData = (ECCSetAutoResponderResponseData)Engine.Engine.ProcessRequest(requestData, 254);

        if (!responseData.IsSuccess)
        {
          Assert.IsFalse(responseData.Response.Item.ResultCode == 0);

          Console.WriteLine("Result Code: " + responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: " + responseData.Response.Item.Timer);
          Console.WriteLine("Message: " + responseData.Response.Item.Message);
        }
        else
        {
          Console.WriteLine("Result Code: " + responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: " + responseData.Response.Item.Timer);
          Console.WriteLine("Message: " + responseData.Response.Item.Message);

          Assert.Fail("There is no way this shopper ID should work.");
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
    public void SetAutoResponderDetailsForShopperWithInvalidEmail()
    {
      ECCSetAutoResponderRequestData requestData = new ECCSetAutoResponderRequestData("847235",
        1,
        "invalid@test.com",
        "Set by Unit Test",
        "Unit test subject.",
        1,
        null,
        null,
        "unitTest@test.com",
        false,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);
      try
      {
        ECCSetAutoResponderResponseData responseData = (ECCSetAutoResponderResponseData)Engine.Engine.ProcessRequest(requestData, 254);

        if (!responseData.IsSuccess)
        {
          Assert.IsFalse(responseData.Response.Item.ResultCode == 0);

          Console.WriteLine("Result Code: " + responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: " + responseData.Response.Item.Timer);
          Console.WriteLine("Message: " + responseData.Response.Item.Message);
        }
        else
        {
          Console.WriteLine("Result Code: " + responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: " + responseData.Response.Item.Timer);
          Console.WriteLine("Message: " + responseData.Response.Item.Message);

          Assert.Fail("There is no way this email should work.");
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
    public void SetAutoResponderDetailsForShopperWithEmbeddedQuotes()
    {
      var shopperId = "858421";
      var email = "smoketest3@imkcars.com";

      ECCSetAutoResponderRequestData requestData = new ECCSetAutoResponderRequestData(
        shopperId,
        1,
        email,
        "Set by Unit Test %22this%22 has quotes ",
        "Unit test %22subject. ",
        1,
        null,
        null,
        "unitTest@test.com",
        false,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);
      try
      {
        ECCSetAutoResponderResponseData responseData = (ECCSetAutoResponderResponseData)Engine.Engine.ProcessRequest(requestData, 254);

        if (!responseData.IsSuccess)
        {
          Assert.IsFalse(responseData.Response.Item.ResultCode == 0);

          Console.WriteLine("Result Code: " + responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: " + responseData.Response.Item.Timer);
          Console.WriteLine("Message: " + responseData.Response.Item.Message);
        }
        else
        {
          Console.WriteLine("Result Code: " + responseData.Response.Item.ResultCode);
          Console.WriteLine("Timer: " + responseData.Response.Item.Timer);
          Console.WriteLine("Message: " + responseData.Response.Item.Message);
          Console.WriteLine(responseData.ResultJson);
        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
