using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface.Constants;
using Atlantis.Framework.ECCGetAddressesForPlanEX.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetAddressesForPlanEX.Tests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void GetEmailAddressesForDomainEX()
    {
      string shopperId = "859775";
      string planGuid  = "048b714d-dd2e-11df-a9c4-0050569575d8";
      int requestType = 344;

      List<string> additionalFields = new List<string> { ECCExtendedDataFields.DELIVERY_MODE, 
                                                         ECCExtendedDataFields.EMAIL_ADDRESS_ID,
                                                         ECCExtendedDataFields.IMAP_STATUS,
                                                         ECCExtendedDataFields.IS_SPAM_FILTER_ON,
                                                         ECCExtendedDataFields.OTHER_LANGUAGE,
                                                         ECCExtendedDataFields.SINGLE_REPLY,
                                                         ECCExtendedDataFields.STATUS,
                                                         ECCExtendedDataFields.USE_SMTP_RELAY,
                                                         ECCExtendedDataFields.VIRUS_RECURRING_ID,
                                                         ECCExtendedDataFields.ALIAS_REDIRECT,
                                                         ECCExtendedDataFields.CATCHALL,
                                                         ECCExtendedDataFields.CATCHALL,
                                                         ECCExtendedDataFields.SINGLE_REPLY};

      ECCGetAddressesForPlanEXRequestData requestData = new ECCGetAddressesForPlanEXRequestData(
        shopperId,
        string.Empty,
        string.Empty,
        string.Empty,
        0,
        1,
        planGuid,
        null,
        false
        );

      try
      {
        ECCGetAddressesForPlanEXResponseData responseData = (ECCGetAddressesForPlanEXResponseData)Engine.Engine.ProcessRequest(requestData, requestType);

        if (!responseData.IsSuccess)
        {
          Assert.Fail("Call was not successful.");
        }
        else
        {
          if (responseData.Response.Item.Results != null && responseData.Response.Item.Results.Count == 1)
          {
            var emailList = responseData.Response.Item.Results[0];

            foreach (var emailItem in emailList)
            {
              Console.WriteLine("Email Address: {0}|Email Type: {1}|Email Status: {2}", emailItem.EmailAddress, emailItem.MailboxType, emailItem.Status);
            }
          }

          Assert.IsNotNull(responseData.ToString());
        }
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
