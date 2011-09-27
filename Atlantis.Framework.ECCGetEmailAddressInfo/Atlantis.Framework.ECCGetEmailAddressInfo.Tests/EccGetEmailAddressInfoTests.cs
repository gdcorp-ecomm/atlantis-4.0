using System;
using System.Diagnostics;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.EccGetEmailAddressInfo.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EccGetEmailAddressInfo.Tests
{
  [TestClass]
  public class EccGetEmailAddressInfoTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EccGetEmailAddressInfo_RequestData()
    {
      string shopperId = "859775";//"858421"; //"85408"; // //"858965"; 
      string emailAddress = "mx1-pend@bitbucket.com";

      int requestType = 233;
      TimeSpan requestTimeout = new TimeSpan(0, 0, 1, 30);

      RequestData requestData = new EccGetEmailAddressInfoRequestData(shopperId,
                                                                                "http://localhost",
                                                                                 Int32.MinValue.ToString(),
                                                                                "localhost",
                                                                                1, 
                                                                                string.Empty, 
                                                                                EmailTypes.All
                                                                                );

      ((EccGetEmailAddressInfoRequestData)requestData).EmailAddress = emailAddress;
      ((EccGetEmailAddressInfoRequestData) requestData).Type = Ecc.Interface.Enums.EmailTypes.All;
      ((EccGetEmailAddressInfoRequestData) requestData).IncludeActiveOnly = false;
      ((EccGetEmailAddressInfoRequestData) requestData).IncludeDynamicData = true;
      ((EccGetEmailAddressInfoRequestData) requestData).ResellerId = "1";


      //((EccGetEmailAddressInfoRequestData)requestData).AccountUid = "12b6982d-48c6-11df-b65b-005056956427";
      //((EccGetEmailAddressInfoRequestData)requestData).CCList = "kklink@godaddy.com, trwalker@godaddy.com";
      //((EccGetEmailAddressInfoRequestData)requestData).EmailAddress = "distro@asdfasdfasdf.com";
      //((EccGetEmailAddressInfoRequestData)requestData).Password = "T3st123!";
      //((EccGetEmailAddressInfoRequestData)requestData).RequestTimeout = new TimeSpan(0, 0, 0, 10);
      //((EccGetEmailAddressInfoRequestData)requestData).ResellerId = 1;
      //((EccGetEmailAddressInfoRequestData)requestData).DiskSpace = 250;

      try
      {
        var setEmailAccountResponseData = (EccGetEmailAddressInfoResponseData)Engine.Engine.ProcessRequest(requestData, requestType);
        Debug.WriteLine(setEmailAccountResponseData.ToXML());
        Assert.IsTrue(setEmailAccountResponseData.IsSuccess);
        //Assert.IsTrue(setEmailAccountResponseData.Response.Item.Results[0].IsCatchAll);
        //Assert.IsTrue(setEmailAccountResponseData.Response.Item.Results[0].HasSpamFilter);

      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
