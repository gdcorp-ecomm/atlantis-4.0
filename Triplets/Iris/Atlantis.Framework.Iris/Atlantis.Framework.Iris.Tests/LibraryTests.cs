using Atlantis.Framework.Iris.Interface.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.Iris.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Iris.Impl.dll")]
  public class LibraryTests
  {
    [TestMethod]
    public void QuickCreateIris()
    {
      int subscriberId = 224;
      string subject = "New entry from unit tests";
      string note = "This is a Test";
      string customerEmail = "kklink@godaddy.com";
      string ipAddress = "127.0.0.1";
      string createdBy = "Mobile Application";
      int privateLabelId = 1;
      string shopperId = "902185";

      var request = new Iris.Interface.QuickCreateIncidentRequestData(subscriberId,subject, note, customerEmail, ipAddress, createdBy, privateLabelId, shopperId);

      var response = Engine.Engine.ProcessRequest(request, 999) as Iris.Interface.QuickCreateIncidentResponseData;

      Assert.IsNotNull(response);
      Assert.IsNotNull(response.ToXML());
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    public void CreateIris()
    {
      int subscriberId = 224;
      string subject = "New entry from unit tests";
      string note = "This is a Test";
      string customerEmail = "kklink@godaddy.com";
      string ipAddress = "127.0.0.1";
      string createdBy = "Mobile Application";
      int privateLabelId = 1;
      string shopperId = "902185";
      int groupId = 1;
      int serviceId = 0;

      var request = new Iris.Interface.CreateIncidentRequestData(subscriberId, subject, note, customerEmail, ipAddress, groupId, serviceId, createdBy, privateLabelId, shopperId);

      var response = Engine.Engine.ProcessRequest(request, 998) as Iris.Interface.CreateIncidentResponseData;

      Assert.IsNotNull(response);
      Assert.IsNotNull(response.ToXML());
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void AddCommentToExistingTicket()
    {
      string note = "More Notes from the unit tests for Adding Comments to existing tickets.";
      long incidentId = 1329192;
      string customerId = "Customer"; 

      var request = new Iris.Interface.AddIncidentNoteRequestData(incidentId, note, customerId);
      var response = Engine.Engine.ProcessRequest(request, 997) as Iris.Interface.AddIncidentNoteResponseData;

      Assert.IsNotNull(response);
      Assert.IsNotNull(response.ToXML());
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);

    }

    [TestMethod]
    public void AddCommentToExistingTicketWithBadIncidentId()
    {
      string note = "More Notes from the unit tests for Adding Comments to existing tickets.";
      long incidentId = -9999999;
      string customerId = "Customer";

      var request = new Iris.Interface.AddIncidentNoteRequestData(incidentId, note, customerId);
      var response = Engine.Engine.ProcessRequest(request, 997) as Iris.Interface.AddIncidentNoteResponseData;

      Assert.IsNotNull(response);
      Assert.IsNotNull(response.ToXML());
      Debug.WriteLine(response.ToXML());
      Assert.IsFalse(response.IsSuccess);

    }

    [TestMethod]
    public void GetIncidentsByShopperIdAndDateRange()
    {

      string shopperId = "902185";
      DateTime startDate =  new DateTime(2013, 10, 1) ;
      DateTime endDate = DateTime.Now;

      var request = new Iris.Interface.GetIncidentsByShopperIdAndDateRangeRequestData(startDate, endDate);
      request.ShopperID = shopperId;
      var response = Engine.Engine.ProcessRequest(request, 996) as Iris.Interface.GetIncidentsByShopperIdAndDateRangeResponseData;

      Assert.IsNotNull(response);
      Assert.IsNotNull(response.ToXML());
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }


    [TestMethod]
    public void GetIncidentCustomerNotes()
    {
      long incidentId = 1329192;
      int noteId = 0;

      var request = new Iris.Interface.GetIncidentCustomerNotesRequestData(incidentId, noteId);
      var response = Engine.Engine.ProcessRequest(request, 995) as Iris.Interface.GetIncidentCustomerNotesResponseData;

      Assert.IsNotNull(response);
      Assert.IsNotNull(response.ToXML());
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }


  }

}
