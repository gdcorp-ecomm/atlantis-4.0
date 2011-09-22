using System;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.ECCGetCalendarDetails.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetCalendarDetails.Tests
{
  [TestClass]
  public class ECCGetCalendarDetailsTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetCalendarAccountDetailsForShopperEmailAccount()
    {
      ECCGetCalendarDetailsRequestData requestData = new ECCGetCalendarDetailsRequestData("859775",
        1,
        false,
        "test@bitbucket.com",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCGetCalendarDetailsResponseData responseData = (ECCGetCalendarDetailsResponseData)Engine.Engine.ProcessRequest(requestData, 257);

        if (!responseData.IsSuccess)
        {
          Assert.Fail("Call was not successful.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.Results[0].AccountStatus == EccCalendarAccountStatus.Active);
          Assert.IsTrue(responseData.Response.Item.Results[0].RecordId == "1797");
          Assert.IsTrue(responseData.Response.Item.Results[0].AccountType == EccAccountType.GroupCalendarPack);
          Assert.IsTrue(responseData.Response.Item.Results[0].FirstName == "test");
          Assert.IsTrue(responseData.Response.Item.Results[0].LastName == "user");
          Assert.IsTrue(responseData.Response.Item.Results[0].Domain == "bitbucket.com");

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
    public void GetCalendarAccountDetailsForShopperEmailAccountNotSetup()
    {
      ECCGetCalendarDetailsRequestData requestData = new ECCGetCalendarDetailsRequestData("859775",
        1,
        false,
        "notsetup@bitbucket.com",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCGetCalendarDetailsResponseData responseData = (ECCGetCalendarDetailsResponseData)Engine.Engine.ProcessRequest(requestData, 257);

        if (!responseData.IsSuccess)
        {
          Assert.Fail("Call was not successful.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.Results[0].AccountStatus == EccCalendarAccountStatus.Pending);
          Assert.IsTrue(responseData.Response.Item.Results[0].RecordId == "0");
          Assert.IsTrue(responseData.Response.Item.Results[0].AccountType == EccAccountType.GroupCalendarPack);
          Assert.IsTrue(responseData.Response.Item.Results[0].FirstName == string.Empty);
          Assert.IsTrue(responseData.Response.Item.Results[0].LastName == string.Empty);
          Assert.IsTrue(responseData.Response.Item.Results[0].Domain == "bitbucket.com");

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
    public void GetCalendarAccountDetailsForInvalidShopper()
    {
      ECCGetCalendarDetailsRequestData requestData = new ECCGetCalendarDetailsRequestData("ASDFASDF",
        1,
        false,
        "test@bitbucket.com",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCGetCalendarDetailsResponseData responseData = (ECCGetCalendarDetailsResponseData)Engine.Engine.ProcessRequest(requestData, 257);

        if (!responseData.IsSuccess)
        {
          Assert.Fail("Call was not successful.");
        }
        else
        {
          Assert.IsFalse(responseData.IsSuccess);
          //Assert.IsTrue(responseData.Response.Item.Message == "");
          Console.WriteLine("Message: " + responseData.Response.Item.Message);
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
    public void GetCalendarAccountDetailsForShopperInvalidEmailAccount()
    {
      ECCGetCalendarDetailsRequestData requestData = new ECCGetCalendarDetailsRequestData("859775",
        1,
        false,
        "asdf@bitbucket.com",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCGetCalendarDetailsResponseData responseData = (ECCGetCalendarDetailsResponseData)Engine.Engine.ProcessRequest(requestData, 257);

        if (!responseData.IsSuccess)
        {
          Assert.Fail("Call was not successful.");
        }
        else
        {
          Assert.IsFalse(responseData.IsSuccess);
          //Assert.IsTrue(responseData.Response.Item.Message == "");
          Console.WriteLine("Message: " + responseData.Response.Item.Message);
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
    public void GetMultipleCalendarAccountDetailsForShopper()
    {
      ECCGetCalendarDetailsRequestData requestData = new ECCGetCalendarDetailsRequestData("859775",
        1,
        false,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCGetCalendarDetailsResponseData responseData = (ECCGetCalendarDetailsResponseData)Engine.Engine.ProcessRequest(requestData, 257);

        if (!responseData.IsSuccess)
        {
          Assert.Fail("Call was not successful.");
        }
        else
        {
          Assert.IsTrue(responseData.Response.Item.Results.Count>0);
          Console.WriteLine("Items returned: {0}",responseData.Response.Item.Results.Count);
          

          Console.WriteLine(responseData.ToXML());

        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
