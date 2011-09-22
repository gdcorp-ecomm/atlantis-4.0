using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface.Constants;
using Atlantis.Framework.ECCGetAddressesForShopperEX.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetAddressesForShopperEX.Tests
{
  [TestClass]
  public class ECCGetAddressesForShopperEXTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetAllEmailAddressesForValidShopper()
    {
      //List<string> additionalFields = new List<string> { ECCExtendedDataFields.DELIVERY_MODE, 
      //                                                   ECCExtendedDataFields.EMAIL_ADDRESS_ID,
      //                                                   ECCExtendedDataFields.IMAP_STATUS,
      //                                                   ECCExtendedDataFields.IS_SPAM_FILTER_ON,
      //                                                   ECCExtendedDataFields.OTHER_LANGUAGE,
      //                                                   ECCExtendedDataFields.SINGLE_REPLY,
      //                                                   ECCExtendedDataFields.STATUS,
      //                                                   ECCExtendedDataFields.USE_SMTP_RELAY,
      //                                                   ECCExtendedDataFields.VIRUS_RECURRING_ID,
      //                                                   ECCExtendedDataFields.ALIAS_REDIRECT,
      //                                                   ECCExtendedDataFields.CATCHALL,
      //                                                   ECCExtendedDataFields.CATCHALL,
      //                                                   ECCExtendedDataFields.SINGLE_REPLY};
      
      ECCGetAddressesForShopperEXRequestData requestData = new ECCGetAddressesForShopperEXRequestData("859775",
        1,
        0,
        false,
        null,
        string.Empty,
        string.Empty,
        string.Empty,
        0);

      try
      {
        ECCGetAddressesForShopperEXResponseData responseData =
          (ECCGetAddressesForShopperEXResponseData)Engine.Engine.ProcessRequest(requestData, 342);

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

          if (responseData.Response.Item.Results != null && responseData.Response.Item.Results.Count == 1)
          {
            var emailList = responseData.Response.Item.Results[0];

            foreach (var emailItem in emailList)
            {
              Console.WriteLine("Email Address: {0}|Email Type: {1}|Email Status: {2}", emailItem.EmailAddress, emailItem.MailboxType, emailItem.Status);
            }
          }
        }

      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
