using System.Diagnostics;
using Atlantis.Framework.AuthTwoFactorDeletePhone.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Auth.Interface;

namespace Atlantis.Framework.AuthTwoFactorDeletePhone.Tests
{
  [TestClass]
  public class GetAuthTwoFactorDeletePhoneTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 513;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthTwoFactorDeletePhoneTest()
    {
      AuthTwoFactorDeletePhoneRequestData request = new AuthTwoFactorDeletePhoneRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , "4805288819"
        , "MyHost"
        , "127.0.0.1");

      AuthTwoFactorDeletePhoneResponseData response = (AuthTwoFactorDeletePhoneResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
    }
  }
}
