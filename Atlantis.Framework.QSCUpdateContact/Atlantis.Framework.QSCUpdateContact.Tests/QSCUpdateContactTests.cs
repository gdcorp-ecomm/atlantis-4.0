using System;
using Atlantis.Framework.QSC.Interface.Constants;
using Atlantis.Framework.QSCUpdateContact.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCUpdateContact.Tests
{
  [TestClass]
  public class QSCUpdateContactTests
  {
    [TestMethod]
    public void FirstContactUpdate()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2074";
      string _contactType = QSCContactTypes.BILL_TO;

			orderContact _contact = new orderContact();
			_contact.firstName = "Thomas";
			_contact.lastName = "Riedy";
			_contact.addressLine1 = "123 Main Street";
			_contact.addressLine2 = "Apt. 24";
			_contact.city = "Phoenix";
			_contact.regionCode = "AZ";
			_contact.postalCode = "85018";
			_contact.countryCode = "US";
			_contact.contactType = QSCContactTypes.BILL_TO;
			_contact.phoneNumber = "4805058800";

      int requestId = 556;

      QSCUpdateContactRequestData request = new QSCUpdateContactRequestData(_shopperId, "",
        string.Empty,
        string.Empty,
        1,
        _accountUid,
        _invoiceId,
				_contact);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCUpdateContactResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCUpdateContactResponseData;

      Assert.IsTrue(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    public void SecondContactUpdateToResetData()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2074";
      int requestId = 556;

			orderContact _contact = new orderContact();
			_contact.firstName = "Ed";
			_contact.lastName = "Testeruser";
			_contact.addressLine1 = "133 E Hollywood Dr";
			_contact.addressLine2 = "Apt. 7738";
			_contact.city = "Hollywood";
			_contact.regionCode = "CA";
			_contact.postalCode = "90028";
			_contact.countryCode = "US";
			_contact.email = "rmartin@godaddy.com";
			_contact.phoneNumber = "(778) 555-5555";
			_contact.contactType = QSCContactTypes.BILL_TO;

      QSCUpdateContactRequestData request = new QSCUpdateContactRequestData(_shopperId, "", 
        string.Empty, 
        string.Empty, 
        1, 
        _accountUid, 
        _invoiceId,
 				_contact);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCUpdateContactResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCUpdateContactResponseData;

      Assert.IsTrue(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }
  }
}
