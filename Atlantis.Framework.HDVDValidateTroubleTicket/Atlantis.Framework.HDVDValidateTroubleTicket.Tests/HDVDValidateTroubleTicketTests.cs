using System;
using Atlantis.Framework.HDVDValidateTroubleTicket.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HDVDValidateTroubleTicket.Tests
{
  [TestClass]
  public class HDVDValidateTroubleTicketTests
  {
    private const string _SHOPPER_ID = "858421";
    private const int _REQUEST_ID = 492;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void NegativeTestWithEmptyValues()
    {
      string firstName = string.Empty;
      string lastName = string.Empty;
      string emailAddress = string.Empty;
      string phoneNumber = string.Empty;
      string summary = string.Empty;
      string details = string.Empty;

      HDVDValidateTroubleTicketRequestData request = new HDVDValidateTroubleTicketRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        firstName,
        lastName,
        emailAddress,
        phoneNumber,
        summary,
        details);

      HDVDValidateTroubleTicketResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDValidateTroubleTicketResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsTrue(response.HasErrors);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PositiveTestWithValues()
    {
      string firstName = "Thomas";
      string lastName = "Riedy";
      string emailAddress = "triedy@GoDaddy.com";
      string phoneNumber = "4805551212";
      string summary = "Need help with my VPH server";
      string details = "How do I reboot?";

      HDVDValidateTroubleTicketRequestData request = new HDVDValidateTroubleTicketRequestData(
        _SHOPPER_ID,
        string.Empty,
        string.Empty,
        string.Empty,
        1,
        firstName,
        lastName,
        emailAddress,
        phoneNumber,
        summary,
        details);

      HDVDValidateTroubleTicketResponseData response =
        Engine.Engine.ProcessRequest(request, _REQUEST_ID) as HDVDValidateTroubleTicketResponseData;

      Console.Out.WriteLine(response.ToXML());
      Assert.IsFalse(response.HasErrors);
    }
  }
}
