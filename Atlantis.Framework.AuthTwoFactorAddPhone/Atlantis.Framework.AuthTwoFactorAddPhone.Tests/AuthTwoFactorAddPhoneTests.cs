using System.Diagnostics;
using Atlantis.Framework.AuthTwoFactorAddPhone.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Auth.Interface;

namespace Atlantis.Framework.AuthTwoFactorAddPhone.Tests
{
  [TestClass]
  public class GetAuthTwoFactorAddPhoneTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 512;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthTwoFactorAddPhoneTest()
    {
      AuthTwoFactorAddPhoneRequestData request = new AuthTwoFactorAddPhoneRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0 
        , "1"
        , "4805288819"
        , "Verizon Wireless"
        , "MyHost"
        , "127.0.0.1");

      AuthTwoFactorAddPhoneResponseData response = (AuthTwoFactorAddPhoneResponseData)Engine.Engine.ProcessRequest(request, _requestType);      
    
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
    }
  }
}
