using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.QSC.Interface.Constants;
using Atlantis.Framework.QSCUpdateContact.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
      int requestId = 556;

      QSCUpdateContactRequestData request = new QSCUpdateContactRequestData(_shopperId, "",
        string.Empty,
        string.Empty,
        1,
        _accountUid,
        _invoiceId,
        null,
        "Thomas",
        null,
        "Riedy",
        null,
        "123 Main Street",
        "Apt. 24",
        "Phoenix",
        "AZ",
        "85018",
        "US",
        "triedy@godaddy.com",
        "(480) 330-4866",
        _contactType);

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
      string _contactType = QSCContactTypes.BILL_TO ;
      int requestId = 556;

      QSCUpdateContactRequestData request = new QSCUpdateContactRequestData(_shopperId, "", 
        string.Empty, 
        string.Empty, 
        1, 
        _accountUid, 
        _invoiceId, 
        null, 
        "Ed", 
        null, 
        "Testeruser", 
        null, 
        "133 E Hollywood Dr", 
        "Apt. 7738", 
        "Hollywood", 
        "CA", 
        "90028", 
        "US", 
        "rmartin@godaddy.com", 
        "(778) 555-5555", 
        _contactType);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCUpdateContactResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCUpdateContactResponseData;

      Assert.IsTrue(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }
  }
}
