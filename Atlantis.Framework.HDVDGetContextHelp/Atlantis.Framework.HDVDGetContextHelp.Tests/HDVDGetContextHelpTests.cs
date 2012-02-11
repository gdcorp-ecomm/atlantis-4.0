using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.HDVDGetContextHelp.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDGetContextHelp.Tests
{
  [TestClass]
  public class HDVDGetContextHelpTests
  {
    private const string _SHOPPER_ID = "858421";
    private const int _REQUEST_ID = 493;

    [TestMethod]
    public void NegativeTestToGetContextHelp()
    {
      const string accountGuid = "d11319d0-4d10-11e1-83a0-0050569575d8";
      string contextHelpPage = "";
      string contextHelpField = "";

      HDVDGetContextHelpRequestData request = new HDVDGetContextHelpRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        contextHelpPage,
        contextHelpField);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDGetContextHelpResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDGetContextHelpResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsTrue(response.Status == "error");
      Assert.IsTrue(response.Title == string.Empty);
      Assert.IsTrue(response.Description == string.Empty);
    }

    [TestMethod]
    public void NegativeTestToGetContextHelpWithNoField()
    {
      const string accountGuid = "d11319d0-4d10-11e1-83a0-0050569575d8";
      string contextHelpPage = "reset-password";
      string contextHelpField = "";

      HDVDGetContextHelpRequestData request = new HDVDGetContextHelpRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        contextHelpPage,
        contextHelpField);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDGetContextHelpResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDGetContextHelpResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsTrue(response.Status == "error");
      Assert.IsTrue(response.Title == string.Empty);
      Assert.IsTrue(response.Description == string.Empty);
    }
  

    // Page='account-summary',
    // Fields='request-additional-ip', 'ftp-backup-ip', 'pending-ram-upgrade'

    //Page='reprovision', 
    // Fields='hostname', 'password', 'username'

    //Page='ftp-backup', 
    // Fields='username', 'password'

    //Page='bandwidth-options', 
    // Fields='change-status', 'suspend', 'change-status-verify', 'suspend-verify'

    //Page='trouble-ticket', 
    // Fields='summary', 'details', 'reboot', 'permission'

    //Page='reset-password'
    // Fields='password'

  [TestMethod]
    public void PositiveTestToGetContextHelp()
    {
      const string accountGuid = "d11319d0-4d10-11e1-83a0-0050569575d8";
      string contextHelpPage = "account-summary";
      string contextHelpField = "request-additional-ip";

      HDVDGetContextHelpRequestData request = new HDVDGetContextHelpRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        contextHelpPage,
        contextHelpField);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDGetContextHelpResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDGetContextHelpResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsTrue(response.Title != string.Empty);
      Assert.IsTrue(response.Description != string.Empty);
    }
  }
}
