using System;
using System.Diagnostics;
using Atlantis.Framework.HDVDSubmitTroubleTicket.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDSubmitTroubleTicket.Tests
{
  [TestClass]
// ReSharper disable InconsistentNaming
  public class HDVDSubmitTroubleTicketTests
// ReSharper restore InconsistentNaming
  {
    private const string _SHOPPER_ID = "858421";
    private const int _REQUEST_ID = 487;


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SubmitTicketWithValidGuid()
    {
      const string accountGuid = "a7c23c7f-3fbb-49a5-99a5-74a0ce8221dc";
      const string firstName = "Tom";
      const string lastName = "Riedy";
      const string custEmail = "triedy@godaddy.com";
      const string custPhone = "4803304866";
      const string ticketTitle = "Test Ticket via Unit Test from Triplet";
      const string ticketBody = "Ticket submitted via Unit Test in Triplet";
      const bool hasBeenRebooted = false;
      const bool grantSupportAccess = true;

      HDVDSubmitTroubleTicketRequestData request = new HDVDSubmitTroubleTicketRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        firstName,
        lastName,
        custEmail,
        custPhone,
        ticketTitle,
        ticketBody,
        hasBeenRebooted,
        grantSupportAccess);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDSubmitTroubleTicketResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDSubmitTroubleTicketResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SubmitTicketWithInvalidGuid()
    {
      const string accountGuid = "stuff";
      const string firstName = "Tom";
      const string lastName = "Riedy";
      const string custEmail = "triedy@godaddy.com";
      const string custPhone = "4803304866";
      const string ticketTitle = "Test Ticket via Unit Test from Triplet";
      const string ticketBody = "Ticket submitted via Unit Test in Triplet";
      const bool hasBeenRebooted = false;
      const bool grantSupportAccess = true;

      HDVDSubmitTroubleTicketRequestData request = new HDVDSubmitTroubleTicketRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        accountGuid,
        firstName,
        lastName,
        custEmail,
        custPhone,
        ticketTitle,
        ticketBody,
        hasBeenRebooted,
        grantSupportAccess);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      HDVDSubmitTroubleTicketResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDSubmitTroubleTicketResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
