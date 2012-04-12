using System.Diagnostics;
using Atlantis.Framework.AuthTwoFactorDisable.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Auth.Interface;

namespace Atlantis.Framework.AuthTwoFactorDisable.Tests
{
  [TestClass]
  public class GetAuthTwoFactorDisableTests
  {
    private const string _shopperId = "Disable_Your_Own_$hit";
    private const int _requestType = 511;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuthTwoFactorDisableTest()
    {
      AuthTwoFactorDisableRequestData request = new AuthTwoFactorDisableRequestData(_shopperId
                                                                                    , string.Empty
                                                                                    , string.Empty
                                                                                    , string.Empty
                                                                                    , 0
                                                                                    , "password"
                                                                                    , "token"
                                                                                    , 1
                                                                                    , "1"
                                                                                    , "4805058800"
                                                                                    , "MeHost"
                                                                                    , "127.0.0.1");

      AuthTwoFactorDisableResponseData response = (AuthTwoFactorDisableResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
    }
  }
}
