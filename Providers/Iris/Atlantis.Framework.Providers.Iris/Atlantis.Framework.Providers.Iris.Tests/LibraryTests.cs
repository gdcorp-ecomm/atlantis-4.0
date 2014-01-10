using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Iris.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.Providers.Iris.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Iris.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Iris.Interface.dll")]
  [DeploymentItem("Atlantis.Framework.Provider.Iris.dll")]
  [DeploymentItem("Atlantis.Framework.Provider.Iris.Interface.dll")]
  public class LibraryTests
  {
    private IProviderContainer SetBasicContextAndProviders()
    {
      var request = new MockHttpRequest("http://www.godaddy.com/");

      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IIrisProvider, IrisProvider>();

      return result;
    }


    [TestMethod]
    public void QuickCreateIris()
    {
      var theContainer = SetBasicContextAndProviders();
      var irisProvider = theContainer.Resolve<IIrisProvider>();

      const int subscriberId = 224;
      const string subject = "New entry from unit tests";
      const string note = "This is a Test";
      const string customerEmail = "kklink@godaddy.com";
      const string ipAddress = "127.0.0.1";
      const string createdBy = "Mobile Application";
      const int privateLabelId = 1;
      const string shopperId = "902185";

      var responseId = irisProvider.QuickCreateIrisIncident(subscriberId, subject, note, customerEmail, ipAddress, createdBy, privateLabelId, shopperId);
      Assert.IsTrue(responseId != -1);

    }

    [TestMethod]
    public void CreateIris()
    {
      var theContainer = SetBasicContextAndProviders();
      var irisProvider = theContainer.Resolve<IIrisProvider>();

      const int subscriberId = 224;
      const string subject = "New entry from unit tests";
      const string note = "This is a Test";
      const string customerEmail = "kklink@godaddy.com";
      const string ipAddress = "127.0.0.1";
      const string createdBy = "Mobile Application";
      const int privateLabelId = 1;
      const string shopperId = "902185";
      const int groupId = 1;
      const int serviceId = 0;

      var incident = irisProvider.CreateIrisIncident(subscriberId, subject, note, customerEmail, ipAddress, createdBy, privateLabelId, groupId, serviceId, shopperId);
      Assert.IsTrue(incident != null);

    }

    [TestMethod]
    public void AddCommentToExistingTicket()
    {
      var theContainer = SetBasicContextAndProviders();
      var irisProvider = theContainer.Resolve<IIrisProvider>();

      string note = "More Notes from the unit tests for Adding Comments to existing tickets.";
      long incidentId = 1329225;
      
      var newNote = irisProvider.AddIncidentNote(incidentId, note, "Customer");
      Assert.IsTrue(newNote != null);
      
    }

    [TestMethod]
    public void AddCommentToExistingTicketWithBadIncidentId()
    {
      var theContainer = SetBasicContextAndProviders();
      var irisProvider = theContainer.Resolve<IIrisProvider>();

      string note = "More Notes from the unit tests for Adding Comments to existing tickets.";
      long incidentId = -9999999;
      string customerId = "Customer";

      var newNote = irisProvider.AddIncidentNote(incidentId, note, "Customer");
      Assert.IsTrue(newNote == null);

    }

    [TestMethod]
    public void GetIncidentsByShopperIdAndDateRange()
    {
      var theContainer = SetBasicContextAndProviders();
      var irisProvider = theContainer.Resolve<IIrisProvider>();

      const string shopperId = "902185";
      var startDate = new DateTime(2013, 10, 1);
      var endDate = DateTime.Now;

      var responseData = irisProvider.GetIncidents(shopperId, startDate, endDate,false);

      Assert.IsNotNull(responseData);
      Assert.IsNotNull(responseData.Count > 0);
      
    }

    [TestMethod]
    public void GetIncidentsByShopperIdAndDateRangeDeepLoad()
    {
      var theContainer = SetBasicContextAndProviders();
      var irisProvider = theContainer.Resolve<IIrisProvider>();

      const string shopperId = "902185";
      var startDate = new DateTime(2013, 10, 1);
      var endDate = DateTime.Now;

      var responseData = irisProvider.GetIncidents(shopperId, startDate, endDate, true);

      Assert.IsNotNull(responseData);
      Assert.IsNotNull(responseData.Count > 0);

    }

    [TestMethod]
    public void GetIncidentCustomerNotes()
    {
      var theContainer = SetBasicContextAndProviders();
      var irisProvider = theContainer.Resolve<IIrisProvider>();

      long incidentId = 1329192;
      int noteId = 1;

      var responseData = irisProvider.GetIncidentNotes(incidentId, noteId);

      Assert.IsNotNull(responseData);
      Assert.IsNotNull(responseData.Count > 0);
    }
  }
}
