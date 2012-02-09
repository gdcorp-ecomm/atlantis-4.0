using System;
using Atlantis.Framework.HDVDSubmitSyncPasswords.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDSubmitSyncPasswords.Tests
{
  [TestClass]
  public class HDVDSubmitSyncPasswordsTests
  {
    private const string _SHOPPER_ID = "858421";
    private const int _REQUEST_ID = 488;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordSyncWithValidGuid()
    {
      const string accountGuid = "a7c23c7f-3fbb-49a5-99a5-74a0ce8221dc";
      const string username = "kklinkdevtest";
      const string password = "Password2";

      HDVDSubmitSyncPasswordsRequestData request = new HDVDSubmitSyncPasswordsRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        username,
        password);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDSubmitSyncPasswordsResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDSubmitSyncPasswordsResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PasswordSyncWithInvalidGuid()
    {
      const string accountGuid = "bad";
      const string username = "kklinkdevtest";
      const string password = "Password3";

      HDVDSubmitSyncPasswordsRequestData request = new HDVDSubmitSyncPasswordsRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        username,
        password);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDSubmitSyncPasswordsResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDSubmitSyncPasswordsResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
