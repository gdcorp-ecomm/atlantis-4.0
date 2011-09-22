using System;
using Atlantis.Framework.ECCGetAutoResponder.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetAutoResponder.Tests
{
  [TestClass]
  public class ECCGetAutoResponderTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetAutoResponderDetailsForShopper()
    {
      ECCGetAutoResponderRequestData requestData = new ECCGetAutoResponderRequestData("847235",
                                                                                      1,
                                                                                      "test@myotherdomain.com",
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      0);

      try
      {
        ECCGetAutoResponderResponseData responseData = (ECCGetAutoResponderResponseData)Engine.Engine.ProcessRequest(requestData, 252);

        if (!responseData.IsSuccess)
        {
          Assert.Fail("Call was not successful.");
        }
        else
        {
          if (responseData.Response.Item.Results[0].Message != null)
          {
            Assert.IsTrue(responseData.Response.Item.Results[0].Message == "Testing123");
            Assert.IsTrue(responseData.Response.Item.Results[0].From == "test@myotherdomain.com");
            Assert.IsTrue(responseData.Response.Item.Results[0].Subject == "Auto response");

            Console.WriteLine("Auto Response Message: " + responseData.Response.Item.Results[0].Message);
            Console.WriteLine("From: " + responseData.Response.Item.Results[0].From);
            Console.WriteLine("Subject: " + responseData.Response.Item.Results[0].Subject);
          }
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
    public void GetAutoResponderDetailsForInvalidShopper()
    {
      ECCGetAutoResponderRequestData requestData = new ECCGetAutoResponderRequestData("asfasdfdsfafdassdffds",
                                                                                      1,
                                                                                      "test@myotherdomain.com",
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      0);

      try
      {
        ECCGetAutoResponderResponseData responseData = (ECCGetAutoResponderResponseData)Engine.Engine.ProcessRequest(requestData, 252);

        Assert.IsFalse(responseData.IsSuccess);
        Assert.IsTrue(responseData.Response.Item.Message == "Email address not valid for supplied shopper.");
        Console.WriteLine("Message: " + responseData.Response.Item.Message);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetAutoResponderDetailsForInvalidEmail()
    {
      ECCGetAutoResponderRequestData requestData = new ECCGetAutoResponderRequestData("847235",
                                                                                      1,
                                                                                      "sadfsdaf#$#$#@$%",
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      string.Empty,
                                                                                      0);

      try
      {
        ECCGetAutoResponderResponseData responseData = (ECCGetAutoResponderResponseData)Engine.Engine.ProcessRequest(requestData, 252);

        Assert.IsFalse(responseData.IsSuccess);
        Assert.IsTrue(responseData.Response.Item.Message == "The username can contain only letters, digits, period, underscore, dash, plus, equals, or hash.\nThe username may not be blank or contain spaces.");
        Console.WriteLine("Message: " + responseData.Response.Item.Message);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
