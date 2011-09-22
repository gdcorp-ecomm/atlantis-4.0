using System;
using Atlantis.Framework.ECCGetAddressesForShopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetAddressesForShopper.Tests
{
  [TestClass]
  public class ECCGetAddressesForShopper
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetAllEmailAddressesForValidShopper()
    {
      ECCGetAddressesForShopperRequestData requestData = new ECCGetAddressesForShopperRequestData("859775",
        1,
        0,
        false,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCGetAddressesForShopperResponseData responseData =
          (ECCGetAddressesForShopperResponseData) Engine.Engine.ProcessRequest(requestData, 291);

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
          //Assert.IsTrue(responseData.Response.Item.Results.Count == 4);
          Console.WriteLine("Items returned: {0}", responseData.Response.Item.Results.Count);
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
    public void GetAllEmailAddressesForInValidShopper()
    {
      ECCGetAddressesForShopperRequestData requestData = new ECCGetAddressesForShopperRequestData("asdfasdf",
        1,
        0,
        false,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCGetAddressesForShopperResponseData responseData =
          (ECCGetAddressesForShopperResponseData)Engine.Engine.ProcessRequest(requestData, 291);

        if (!responseData.IsSuccess)
        {
          Console.WriteLine("ResultCode: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Assert.IsTrue(!responseData.IsSuccess);
        }
        else
        {
          Console.WriteLine("ResultCode: {0}", responseData.Response.Item.ResultCode);
          Console.WriteLine("Message: {0}", responseData.Response.Item.Message);
          Console.WriteLine("Timer: {0}", responseData.Response.Item.Timer);
          Console.WriteLine("Items returned: {0}", responseData.Response.Item.Results.Count);
          Console.WriteLine(responseData.ToXML());
          if (responseData.Addresses.Count == 0)
          {
            Console.WriteLine("There are no addresses returned by the service for this shopper.");
          }
          Assert.IsTrue(responseData.Addresses.Count == 0);
          Assert.IsTrue(responseData.Domains == null);
        }

      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
