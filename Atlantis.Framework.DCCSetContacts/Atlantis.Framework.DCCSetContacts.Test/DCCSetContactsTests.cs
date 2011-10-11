using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCSetContacts.Interface;


namespace Atlantis.Framework.DCCSetContacts.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DCCSetContactsTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCSetContactsAll()
    {
      DCCSetContactsRequestData request = new DCCSetContactsRequestData("84566", string.Empty, string.Empty, string.Empty, 0, 1, 1532283, "MOBILE_CSA_DCC");
      //(registrant = 0, technical = 1, admin = 2, billing = 3)

      Dictionary<string, string> oRegContact = new Dictionary<string, string>();
      oRegContact.Add("Type", "0");
      oRegContact.Add("FirstName", "Simon");
      oRegContact.Add("LastName", "Birch");
      oRegContact.Add("Company", "Jack of Hearts");
      oRegContact.Add("Address1", "123 N 123 St");
      oRegContact.Add("Address2", "Suite 101");
      oRegContact.Add("City", "Scottsdale");
      oRegContact.Add("State", "Arizona");
      oRegContact.Add("Zip", "85250");
      oRegContact.Add("Country", "US");
      oRegContact.Add("Phone", "+1.4805556666");
      oRegContact.Add("Fax", "+1.4806667777");
      oRegContact.Add("Email", "dcasey@godaddy.com");
      request.addContact(0, oRegContact);

      Dictionary<string, string> oTechContact = new Dictionary<string, string>();
      oTechContact.Add("Type", "1");
      oTechContact.Add("FirstName", "Simon");
      oTechContact.Add("LastName", "Birch");
      oTechContact.Add("Company", "Jack of Hearts");
      oTechContact.Add("Address1", "123 N 123 St");
      oTechContact.Add("Address2", "Suite 101");
      oTechContact.Add("City", "Scottsdale");
      oTechContact.Add("State", "Arizona");
      oTechContact.Add("Zip", "85250");
      oTechContact.Add("Country", "US");
      oTechContact.Add("Phone", "4805556666");
      oTechContact.Add("Fax", "4806667777");
      oTechContact.Add("Email", "dcasey@godaddy.com");
      request.addContact(1, oTechContact);

      Dictionary<string, string> oAdminContact = new Dictionary<string, string>();
      oAdminContact.Add("Type", "2");
      oAdminContact.Add("FirstName", "Simon");
      oAdminContact.Add("LastName", "Birch");
      oAdminContact.Add("Company", "Jack of Hearts");
      oAdminContact.Add("Address1", "123 N 123 St");
      oAdminContact.Add("Address2", "Suite 101");
      oAdminContact.Add("City", "Scottsdale");
      oAdminContact.Add("State", "Arizona");
      oAdminContact.Add("Zip", "85250");
      oAdminContact.Add("Country", "US");
      oAdminContact.Add("Phone", "4805556666");
      oAdminContact.Add("Fax", "4806667777");
      oAdminContact.Add("Email", "dcasey@godaddy.com");
      request.addContact(2, oAdminContact);

      Dictionary<string, string> oBillingContact = new Dictionary<string, string>();
      oBillingContact.Add("Type", "3");
      oBillingContact.Add("FirstName", "Simon");
      oBillingContact.Add("LastName", "Birch");
      oBillingContact.Add("Company", "Jack of Hearts");
      oBillingContact.Add("Address1", "123 N 123 St");
      oBillingContact.Add("Address2", "Suite 101");
      oBillingContact.Add("City", "Scottsdale");
      oBillingContact.Add("State", "Arizona");
      oBillingContact.Add("Zip", "85250");
      oBillingContact.Add("Country", "US");
      oBillingContact.Add("Phone", "4805556666");
      oBillingContact.Add("Fax", "4806667777");
      oBillingContact.Add("Email", "dcasey@godaddy.com");
      request.addContact(3, oBillingContact);

      DCCSetContactsResponseData response = (DCCSetContactsResponseData)Engine.Engine.ProcessRequest(request, 103);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCSetContactsRegistrant()
    {
      DCCSetContactsRequestData request = new DCCSetContactsRequestData("87409", string.Empty, string.Empty, string.Empty, 0, 1, 1549356, "MOBILE_CSA_DCC");
      //(registrant = 0, technical = 1, admin = 2, billing = 3)

      Dictionary<string, string> oRegContact = new Dictionary<string, string>();
      oRegContact.Add("Type", "0");
      oRegContact.Add("FirstName", "Simon");
      oRegContact.Add("LastName", "Birch");
      oRegContact.Add("Company", "Jack of Hearts");
      oRegContact.Add("Address1", "123 N 123 St");
      oRegContact.Add("Address2", "Suite 101");
      oRegContact.Add("City", "Scottsdale");
      oRegContact.Add("State", "Arizona");
      oRegContact.Add("Zip", "85250");
      oRegContact.Add("Country", "US");
      oRegContact.Add("Phone", "+1.4805556666");
      oRegContact.Add("Fax", "+1.4806667777");
      oRegContact.Add("Email", "dcasey@godaddy.com");
      request.addContact(0, oRegContact);

      DCCSetContactsResponseData response = (DCCSetContactsResponseData)Engine.Engine.ProcessRequest(request, 103);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCSetContactsForShopperThatDoesNotOwnTheDomain()
    {
      DCCSetContactsRequestData request = new DCCSetContactsRequestData("847235", string.Empty, string.Empty, string.Empty, 0, 1, 1532283, "MOBILE_CSA_DCC");
      //(registrant = 0, technical = 1, admin = 2, billing = 3)

      Dictionary<string, string> oRegContact = new Dictionary<string, string>();
      oRegContact.Add("Type", "0");
      oRegContact.Add("FirstName", "Simon");
      oRegContact.Add("LastName", "Birch");
      oRegContact.Add("Company", "Jack of Hearts");
      oRegContact.Add("Address1", "123 N 123 St");
      oRegContact.Add("Address2", "Suite 101");
      oRegContact.Add("City", "Scottsdale");
      oRegContact.Add("State", "Arizona");
      oRegContact.Add("Zip", "85250");
      oRegContact.Add("Country", "US");
      oRegContact.Add("Phone", "+1.4805556666");
      oRegContact.Add("Fax", "+1.4806667777");
      oRegContact.Add("Email", "dcasey@godaddy.com");
      request.addContact(0, oRegContact);

      DCCSetContactsResponseData response = (DCCSetContactsResponseData)Engine.Engine.ProcessRequest(request, 103);

      // This is returning success true, the DCC team is fixing this
      Assert.IsFalse(response.IsSuccess);
    }
  }
}