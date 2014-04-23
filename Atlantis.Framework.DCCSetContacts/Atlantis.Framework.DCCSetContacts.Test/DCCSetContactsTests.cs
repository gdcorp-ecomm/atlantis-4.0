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
    [DeploymentItem("Atlantis.Framework.DCCSetContacts.Impl.dll")]
    public void DCCSetContactsAll()
    {
      var request = new DCCSetContactsRequestData("857020", string.Empty, string.Empty, string.Empty, 0, 1, 1665465, "MOBILE_CSA_DCC");
      //(registrant = 0, technical = 1, admin = 2, billing = 3)

      var oRegContact = new Dictionary<string, string>
      {
        {"Type", "0"},
        {"FirstName", "Simon"},
        {"LastName", "Birch"},
        {"Company", "Jack of Hearts"},
        {"Address1", "123 N 123 St"},
        {"Address2", "Suite 101"},
        {"City", "Scottsdale"},
        {"State", "Arizona"},
        {"Zip", "85250"},
        {"Country", "US"},
        {"Phone", "+1.4805556666"},
        {"Fax", "+1.4806667777"},
        {"Email", "dcasey@godaddy.com"}
      };
      request.addContact(0, oRegContact);

      var oTechContact = new Dictionary<string, string>
      {
        {"Type", "1"},
        {"FirstName", "Simon"},
        {"LastName", "Birch"},
        {"Company", "Jack of Hearts"},
        {"Address1", "123 N 123 St"},
        {"Address2", "Suite 101"},
        {"City", "Scottsdale"},
        {"State", "Arizona"},
        {"Zip", "85250"},
        {"Country", "US"},
        {"Phone", "4805556666"},
        {"Fax", "4806667777"},
        {"Email", "dcasey@godaddy.com"}
      };
      request.addContact(1, oTechContact);

      var oAdminContact = new Dictionary<string, string>
      {
        {"Type", "2"},
        {"FirstName", "Simon"},
        {"LastName", "Birch"},
        {"Company", "Jack of Hearts"},
        {"Address1", "123 N 123 St"},
        {"Address2", "Suite 101"},
        {"City", "Scottsdale"},
        {"State", "Arizona"},
        {"Zip", "85250"},
        {"Country", "US"},
        {"Phone", "4805556666"},
        {"Fax", "4806667777"},
        {"Email", "dcasey@godaddy.com"}
      };
      request.addContact(2, oAdminContact);

      var oBillingContact = new Dictionary<string, string>
      {
        {"Type", "3"},
        {"FirstName", "Simon"},
        {"LastName", "Birch"},
        {"Company", "Jack of Hearts"},
        {"Address1", "123 N 123 St"},
        {"Address2", "Suite 101"},
        {"City", "Scottsdale"},
        {"State", "Arizona"},
        {"Zip", "85250"},
        {"Country", "US"},
        {"Phone", "4805556666"},
        {"Fax", "4806667777"},
        {"Email", "dcasey@godaddy.com"}
      };
      request.addContact(3, oBillingContact);

      var response = (DCCSetContactsResponseData)Engine.Engine.ProcessRequest(request, 103);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCSetContactsRegistrant()
    {
      DCCSetContactsRequestData request = new DCCSetContactsRequestData("857020", string.Empty, string.Empty, string.Empty, 0, 1, 1665465, "MOBILE_CSA_DCC");
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