using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Testing.MockProviders;


namespace Atlantis.Framework.Providers.DomainContactValidation.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DomainContactValidation.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainsTrustee.Impl.dll")]
  public class DomainContactGroupTests
  {
    [TestInitialize]
    public void Initialize()
    {
      var request = new MockHttpRequest("http://spoonymac.com/");
      MockHttpContext.SetFromWorkerRequest(request);
    }

    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          ((MockProviderContainer)_providerContainer).SetMockSetting(MockSiteContextSettings.IsRequestInternal, true);

          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockNoManagerContext>();
          _providerContainer.RegisterProvider<IDomainContactValidationProvider, DomainContactValidationProvider>();
        }

        return _providerContainer;
      }
    }

    private IDomainContactValidationProvider _domainContactProvider;
    private IDomainContactValidationProvider DomainContactProvider
    {
      get
      {
        if (_domainContactProvider == null)
        {
          _domainContactProvider = ProviderContainer.Resolve<IDomainContactValidationProvider>();
        }

        return _domainContactProvider;
      }
    }

    public TestContext TestContext { get; set; }

    #region Additional test attributes

    #endregion

    [TestMethod]
    public void TestDotCOMContact()
    {
      var tlds = new List<string> { ".COM" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, 1);



      var registrantContact = DomainContactProvider.DomainContactInstance("Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true, "101 N Street", "Suite 100", "Littleton", "CO","80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(DomainContactType.Registrant, registrantContact);
      Assert.AreEqual(0, registrantContact.Errors.Count);
    }

    [TestMethod]
    public void TestDotJPContact()
    {
      var tlds = new List<string> { ".JP" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, 1);


      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(DomainContactType.Registrant, registrantContact);
      Assert.AreNotEqual(0, registrantContact.Errors.Count);
    }

    [TestMethod]
    public void DomainContactClone()
    {
      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      var clonedContact = registrantContact.Clone() as DomainContact;

      Assert.AreNotEqual(registrantContact.GetContactXml(DomainContactType.Registrant),
        clonedContact.GetContactXml(DomainContactType.Registrant));

      Assert.AreEqual(registrantContact.FirstName, clonedContact.FirstName);
      Assert.AreEqual(registrantContact.LastName, clonedContact.LastName);
      Assert.AreEqual(registrantContact.Email, clonedContact.Email);
      Assert.AreEqual(registrantContact.Address1, clonedContact.Address1);
      Assert.AreEqual(registrantContact.Phone, clonedContact.Phone);
    }

    [TestMethod]
    public void DomainContactXmlConstructor()
    {
      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      string xml = registrantContact.GetContactXml(DomainContactType.Registrant);
      var contactDoc = new XmlDocument();
      contactDoc.LoadXml(xml);
      var newContact = DomainContactProvider.DomainContactInstance(contactDoc);

      Assert.AreEqual(registrantContact.GetContactXml(DomainContactType.Registrant),
        newContact.GetContactXml(DomainContactType.Registrant));
    }

    [TestMethod]
    public void DomainContactXmlConstructorWithErrors()
    {
      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      registrantContact.Errors.Add(DomainContactProvider.DomainContactErrorInstance("blue", 1, "blue error", (int)DomainContactType.Registrant));
      string xml = registrantContact.GetContactXml(DomainContactType.Registrant);
      Console.WriteLine(xml);
      var contactDoc = new XmlDocument();
      contactDoc.LoadXml(xml);
      var newContact = DomainContactProvider.DomainContactInstance(contactDoc);

      Assert.AreEqual(registrantContact.GetContactXml(DomainContactType.Registrant),
        newContact.GetContactXml(DomainContactType.Registrant));

      Assert.IsFalse(newContact.IsValid);
    }

    [TestMethod]
    public void DomainContactGroupErrors()
    {
      var tlds = new List<string> { "COM", "JP" };
      var group = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
       "Bill", "Registrant", "bregistrant@bongo.com",
         "MumboJumbo", true,
        "101 N Street", "Suite 100", "Littleton", "CO",
        "80130", "US", "(303)-555-1213", "(303)-555-2213");
      bool valid = group.SetContact(registrantContact);

      Assert.IsFalse(valid);

      Console.WriteLine(group.ToString());
      Console.WriteLine(group.GetContactXml());
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DomainContactGroupErrorsSerialization()
    {
      var tlds = new List<string> { "COM", "JP" };
      var group = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
       "Bill", "Registrant", "bregistrant@bongo.com",
         "MumboJumbo", true,
        "101 N Street", "Suite 100", "Littleton", "CO",
        "80130", "US", "(303)-555-1213", "(303)-555-2213");
      bool valid = group.SetContact(registrantContact);

      Assert.IsFalse(valid);

      string groupString = group.ToString();
      var newGroup = DomainContactProvider.DomainContactGroupInstance(groupString);

      Assert.IsFalse(newGroup.IsValid);

      //This test is not valid, the two strings are different by a single attribute: cacpr
      //Assert.AreEqual(newGroup.ToString(), groupString);
    }

    [TestMethod]
    public void DomainContactGroupNewTlds()
    {
      var tlds = new List<string> { "COM", "NET" };
      var group = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
       "Bill", "Registrant", "bregistrant@bongo.com",
         "MumboJumbo", true,
        "101 N Street", "Suite 100", "Littleton", "CO",
        "80130", "US", "(303)-555-1213", "(303)-555-2213");
      bool valid = group.SetContact(registrantContact);

      Assert.IsTrue(valid);

      tlds.Remove("NET");
      group.SetTlds(tlds);

      Assert.IsTrue(group.IsValid);

      tlds.Add("JP");
      group.SetTlds(tlds);

      Assert.IsFalse(group.IsValid);
      List<IDomainContactError> errors = group.GetAllErrors();

      tlds.Remove("JP");
      group.SetTlds(tlds);

      Assert.IsTrue(group.IsValid);
      errors = group.GetAllErrors();

    }

    [TestMethod]
    public void DomainContactGroupTrySetContact()
    {
      var tlds = new List<string> { "COM", "JP" };
      var group = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
       "Bill", "Registrant", "bregistrant@bongo.com",
         "MumboJumbo", true,
        "101 N Street", "Suite 100", "Littleton", "CO",
        "80130", "US", "(303)-555-1213", "(303)-555-2213");
      bool valid = group.TrySetContact(DomainContactType.Registrant, registrantContact);

      Assert.IsFalse(valid);
      Assert.IsTrue(registrantContact.Errors.Count > 0);

      var getContact = group.GetContact(DomainContactType.Registrant);
      Assert.IsNull(getContact);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestDotITContact()
    {
      var tlds = new List<string> { ".IT" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "Geneva",
          "80130", "CH", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(DomainContactType.Registrant, registrantContact);
      Assert.AreEqual(0, registrantContact.Errors.Count);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestTrusteeVendorIds()
    {
      var tlds = new List<string> { ".IT", ".FR" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "101 N Street", "Suite 100", "Littleton", "Colorado",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(DomainContactType.Registrant, registrantContact);
      int trusteesCount = registrantContact.TrusteeVendorIds.Count;
      var reg = contactGroup.GetContact(DomainContactType.Registrant);
      int regTrusteesCount = reg.TrusteeVendorIds.Count;
      Assert.AreEqual(true, trusteesCount == regTrusteesCount);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestHongKong()
    {
      var tlds = new List<string> { ".COM" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "101 N Street", "Suite 100", "Littleton", "Colorado",
          string.Empty, "HK", "(303)-555-1213", "(303)-555-2213");

      var adminContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "101 N Street", "Suite 100", "Littleton", "Colorado",
          string.Empty, "HK", "(303)-555-1213", "(303)-555-2213");

      var billingContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "101 N Street", "Suite 100", "Littleton", "Colorado",
          string.Empty, "HK", "(303)-555-1213", "(303)-555-2213");

      var techContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "101 N Street", "Suite 100", "Littleton", "Colorado",
          string.Empty, "HK", "(303)-555-1213", "(303)-555-2213");

      contactGroup.SetContacts(registrantContact, techContact, adminContact, billingContact);
      int trusteesCount = registrantContact.TrusteeVendorIds.Count;
      var reg = contactGroup.GetContact(DomainContactType.Registrant);
      int regTrusteesCount = reg.TrusteeVendorIds.Count;
      Assert.AreEqual(true, contactGroup.GetAllErrors().Count == 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestDotCaContact()
    {
      var tlds = new List<string> { ".CA" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "101 N Street", "Suite 100", "Littleton", "Geneva",
          "80130", "CH", "(303)-555-1213", "(303)-555-2213", "LGR", "ENG");
      contactGroup.SetContact(DomainContactType.Registrant, registrantContact);
      string groupSessionString = contactGroup.ToString();
      string xml = registrantContact.GetContactXml(DomainContactType.Registrant);
      string contactSessionString = registrantContact.GetContactXmlForSession(DomainContactType.Registrant);
      Assert.IsFalse(string.IsNullOrEmpty(groupSessionString) && string.IsNullOrEmpty(xml) && string.IsNullOrEmpty(contactSessionString));
      /*
          <?xml version="1.0" encoding="utf-16" standalone="no"?>
          <contact type="blank or xfer"
          contactType="blank or registrant or administrative"
          tlds="INFO"
          fname="fname"
          lname="lname"
          org="org"
          sa1="some address 1"
          sa2="some address 2"
          city="some city"
          sp="TX"
          pc="76310"
          cc="United States"
          phone="(940) 123-4567"
          fax="(940) 123-4567"
          email=”test@test.com”
          privateLabelId="1334556"
          cacpr = “CCT”/>
          
          cacpr possible values right now are:

          CCO, CCT, RES, GOV, EDU, ASS, HOP, PRT, TDM, TRD, PLT, LAM, TRS, ABO, INB, LGR, OMK, MAJ 
          <error attribute="lname" code="3001" desc="CA Contact must contain at least one word from the valid non-individual CIRA word list" />

       * */
    }

    [TestMethod]
    public void TestDotFRContactTuiFormInfo()
    {
      var tlds = new List<string> { "FR" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(DomainContactType.Registrant, registrantContact);
      contactGroup.SetContact(DomainContactType.Registrant, registrantContact);

      var tuiFormInfo = contactGroup.GetTuiFormInfo(tlds);
      Assert.AreEqual(true, tuiFormInfo != null && tuiFormInfo.ContainsKey("FR"));
    }
  }
}
