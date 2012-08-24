using System;
using Atlantis.Framework.MobileAir2WebSms.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MobileAir2WebSms.Tests
{
  [TestClass]
  public class MobileAir2WebSmsTests
  {
    [TestMethod]
    public void SendSmsMessageToPhone()
    {
      string phoneNumber = "14807605267"; // Change this or you will spam Tim Walker's cell :o)
      MobileAir2WebSmsRequestData requestData = new MobileAir2WebSmsRequestData(phoneNumber, 
                                                                                "Atlantis.Framework.MobileAir2WebSms.Tests.MobileAir2WebSmsTests UnitTest", 
                                                                                string.Empty, 
                                                                                string.Empty, 
                                                                                string.Empty, 
                                                                                string.Empty, 
                                                                                0);

      MobileAir2WebSmsResponseData responseData = (MobileAir2WebSmsResponseData)Engine.Engine.ProcessRequest(requestData, 307);

      Console.WriteLine("Response Code: " + responseData.ResponseCode);
      Console.WriteLine("Response Message: " + responseData.ResponseMessage);
      Assert.IsTrue(responseData.IsSuccess);
    }
  }
}
