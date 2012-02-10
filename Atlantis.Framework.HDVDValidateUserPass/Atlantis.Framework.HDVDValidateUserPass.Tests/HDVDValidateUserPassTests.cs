using System;
using Atlantis.Framework.HDVDValidateUserPass.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDValidateUserPass.Tests
{
  [TestClass]
  public class HDVDValidateUserPassTests
  {
    private const string _SHOPPER_ID = "858421";
    private const int _REQUEST_ID = 491;
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PositiveTestToValidatePassword()
    {
      const string accountGuid = "d11319d0-4d10-11e1-83a0-0050569575d8";
      const string hostName = "TESTSERVER02";
      const string username = "kklinkdevtest";
      const string password = "APassword2";
      const bool validateFtp = false;
      const string ftpUserName = "";
      const string ftpPassword = "";
      const string firewallPassword = "";

      HDVDValidateUserPassRequestData request = new HDVDValidateUserPassRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        hostName,
        username,
        password,
        validateFtp,
        ftpUserName,
        ftpPassword,
        firewallPassword);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDValidateUserPassResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDValidateUserPassResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsFalse(response.HasErrors);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void NegativeTestToValidatePassword()
    {
      const string accountGuid = "d11319d0-4d10-11e1-83a0-0050569575d8";
      const string hostName = "TESTSERVER02";
      const string username = "kklinkdevtest";
      const string password = "password";
      const bool validateFtp = false;
      const string ftpUserName = "";
      const string ftpPassword = "";
      const string firewallPassword = "";

      HDVDValidateUserPassRequestData request = new HDVDValidateUserPassRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        hostName,
        username,
        password,
        validateFtp,
        ftpUserName,
        ftpPassword,
        firewallPassword);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDValidateUserPassResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDValidateUserPassResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsTrue(response.HasErrors);
    }
  }
}
