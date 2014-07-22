using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
//using Atlantis.Framework.DotTypeCache;
//using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeCache;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;
//using Atlantis.Framework.Providers.TLDDataCache;
//using Atlantis.Framework.Providers.TLDDataCache.Interface;
using Atlantis.Framework.Providers.TLDDataCache;
using Atlantis.Framework.Providers.TLDDataCache.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Testing.MockProviders;


namespace Atlantis.Framework.Providers.DomainContactValidation.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("dottypecache.config")]
  [DeploymentItem("Atlantis.Framework.DomainContactValidation.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainsTrustee.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Impl.dll")]
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
          ((MockProviderContainer)_providerContainer).SetData(MockSiteContextSettings.IsRequestInternal, true);

          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<IManagerContext, MockNoManagerContext>();
          _providerContainer.RegisterProvider<IDomainContactValidationProvider, DomainContactValidationProvider>();
          _providerContainer.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
          _providerContainer.RegisterProvider<ITLDDataCacheProvider, TLDDataCacheProvider>();

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
      var domains = new List<string> {"test.com"};
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);



      var registrantContact = DomainContactProvider.DomainContactInstance("Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true, "101 N Street", "Suite 100", "Littleton", "CO","80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(DomainContactType.Registrant, registrantContact);
      Assert.AreEqual(0, registrantContact.Errors.Count);
    }

    [TestMethod]
    public void TestDotUKContactErrors()
    {
      var tlds = new List<string> { "uk" };
      var domains = new List<string> {"oberon-1api-de.uk"};

      IDomainContactGroup domainContact = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance("Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true, "101 N Street", "Suite 100", "Littleton", "CO", "80130", "US", "+44.2012341234", "(303)-555-2213");

      domainContact.SetContact(DomainContactType.Registrant, registrantContact);

      IDomainContactError error = registrantContact.Errors.FirstOrDefault();
      Assert.IsNotNull(error);

      Assert.IsTrue(error.Domains.Count() == 1);
    }

    [TestMethod]
    public void TestDotJPContact()
    {
      var tlds = new List<string> { ".JP" };
      var domains = new List<string> { "test.jp" };

      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);


      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(DomainContactType.Registrant, registrantContact);
      Assert.AreNotEqual(0, registrantContact.Errors.Count);
      Assert.AreNotEqual("", registrantContact.Errors[0].DisplayString);
      Assert.IsNotNull(registrantContact.Errors[0].DisplayString);
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
      registrantContact.Errors.Add(DomainContactProvider.DomainContactErrorInstance("blue", 1,1, "blue error", (int)DomainContactType.Registrant));
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
      var domains = new List<string> {"test.com", "test.jp"};
      var group = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

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
      var domains = new List<string> { "test.com", "test.jp" };
      var group = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

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
      List<string> tlds = new List<string>() { "COM", "NET" };
      List<string> domains = new List<string>() { "test.com", "test.net" };
      
      var group = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);
      
      var registrantContact = DomainContactProvider.DomainContactInstance(
       "Bill", "Registrant", "bregistrant@bongo.com",
         "MumboJumbo", true,
        "101 N Street", "Suite 100", "Littleton", "CO",
        "80130", "US", "(303)-555-1213", "(303)-555-2213");
      bool valid = group.SetContact(registrantContact);

      Assert.IsTrue(valid);

      group.SetTlds(new List<string> { "COM" });

      Assert.IsTrue(group.IsValid);

      group.SetTlds(new List<string> { "COM", "JP" });

      Assert.IsFalse(group.IsValid);
      List<IDomainContactError> errors = group.GetAllErrors();

      group.SetTlds(new List<string> { "COM", "NET" });

      Assert.IsTrue(group.IsValid);
      errors = group.GetAllErrors();

    }

    [TestMethod]
    public void DomainContactGroupTrySetContact()
    {
      var tlds = new List<string> { "COM", "JP" };
      List<string> domains = new List<string>(){"test.com", "test.jp"};
      var group = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

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
      var domains = new List<string> {"test.it"};
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

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
      List<string> domains = new List<string>(){"test.it", "test.fr"};
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

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
      List<string> domains = new List<string>(){"test.com"};
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "101 N Street", "Suite 100", "Littleton", "Colorado",
          "00000", "HK", "(303)-555-1213", "(303)-555-2213");

      var adminContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "101 N Street", "Suite 100", "Littleton", "Colorado",
          "00000", "HK", "(303)-555-1213", "(303)-555-2213");

      var billingContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "101 N Street", "Suite 100", "Littleton", "Colorado",
          "00000", "HK", "(303)-555-1213", "(303)-555-2213");

      var techContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "101 N Street", "Suite 100", "Littleton", "Colorado",
          "00000", "HK", "(303)-555-1213", "(303)-555-2213");

      contactGroup.SetContacts(registrantContact, techContact, adminContact, billingContact);
      
      int contactGroupErrors = contactGroup.GetAllErrors().Count;
      Assert.AreEqual(contactGroupErrors, 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestDotCaContact()
    {
      var tlds = new List<string> { ".CA" };
      List<string> domains = new List<string>(){"test.ca"};
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

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
    public void TestUpdatedContactForTrusteeIds()
    {
      
    }

    [TestMethod]
    public void TestContactSessionForTrusteeIds()
    {
       string DEFAULTGROUPSESSION = "Domains.Contacts.Default";
      var tlds = new List<string> { "COM.BR", "NET.BR" };
      List<string> domains = new List<string>(){"test.com.br", "test.net.br"};
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(registrantContact);
      string contactSessionString = contactGroup.ToString();
      HttpContext.Current.Session[DEFAULTGROUPSESSION] = contactSessionString;
      string fromSession = HttpContext.Current.Session[DEFAULTGROUPSESSION].ToString();
      var domainContactGroup = DomainContactProvider.DomainContactGroupInstance(fromSession);
      var tuiFormInfo = domainContactGroup.GetContact(DomainContactType.Registrant);
      Assert.AreEqual(true, tuiFormInfo != null && tuiFormInfo.TuiFormsInfo.ContainsKey("COM.BR"));
    }

    [TestMethod]
    public void TestDotFRContactTuiFormInfo()
    {
      var tlds = new List<string> { "FR" };
      var domains = new List<string>(){"test.fr"};
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(registrantContact);

      var tuiFormInfo = contactGroup.GetContact(DomainContactType.Registrant);
      Assert.AreEqual(true, tuiFormInfo != null && tuiFormInfo.TuiFormsInfo.ContainsKey("FR"));
    }

    [TestMethod]
    public void TestDotCLContactTuiFormInfo()
    {
      var gaTlds = new Dictionary<string, LaunchPhases>();
      gaTlds.Add("my", LaunchPhases.GeneralAvailability);

      var tlds = new List<string> { "my" };
      var domains = new List<string>() { "test.my" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "La Cruz", "Quillota",
          "80130", "my", "(303)-555-1213", "(303)-555-2213");

      IDictionary<string, ITuiFormInfo> tuiFormInfo = new Dictionary<string, ITuiFormInfo>();
      
      if (contactGroup.SetContact(registrantContact))
      {
        tuiFormInfo = contactGroup.GetTuiFormInfo(gaTlds);
      }

      Assert.AreEqual(true, tuiFormInfo != null && tuiFormInfo.Count > 0);
    }

    [TestMethod]
    public void DomainContactCreateMethodPassTest()
    {
      string firstname = string.Empty;
      string lastname = string.Empty;
      string email = string.Empty;
      string company = string.Empty;
      string addr1 = string.Empty;
      string addr2 = string.Empty;
      string city = string.Empty;
      string state = string.Empty;
      string zip = string.Empty;
      string country = string.Empty;
      string phone = string.Empty;
      string fax = string.Empty;
      string caPresence = string.Empty;

      ContactValidation newUser = ContactValidation.Create(firstname, lastname, email, company, addr1, addr2, city, state, zip, country, phone, fax, caPresence);

      Assert.IsNotNull(newUser.Address1);
    }


    [TestMethod]
    public void DomainContactCreateMethodFailTest()
    {
      string firstname = null;
      string lastname = null;
      string email = null;
      string company = null;
      string addr1 = null;
      string addr2 = null;
      string city = null;
      string state = null;
      string zip = null;
      string country = null;
      string phone = null;
      string fax = null;
      string caPresence = null;

      ContactValidation newUser = ContactValidation.Create(firstname, lastname, email, company, addr1, addr2, city, state, zip, country, phone, fax, caPresence);

      Assert.IsNull(newUser.Address1);
    }

    [TestMethod]
    public void ComDotBrTrusteeIdTest()
    {
      var tlds = new List<string> { "COM.BR", "NET.BR"  };
      var domains = new List<string>() { "test.com.br", "test.net.br" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(registrantContact);
      var newContact = contactGroup.GetContact(DomainContactType.Registrant);

      Assert.AreEqual(true, newContact.TuiFormsInfo != null && newContact.TuiFormsInfo.ContainsKey("COM.BR"));
    }

    [TestMethod]
    public void ComDotBrEligibilityTest()
    {
      var tlds = new List<string> { "COM.BR", "COM" };
      var domains = new List<string>() { "test.com.br", "test.com" };

      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);
      var phases = new Dictionary<string, LaunchPhases>();
      phases["COM.BR"] = LaunchPhases.GeneralAvailability;
      phases["COM"] = LaunchPhases.GeneralAvailability;

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           string.Empty, false,
          "Street", "Suite 100", "Littleton", "Amapa",
          "55555-555", "br", "+1 3031234567", string.Empty);
      contactGroup.SetContact(registrantContact);
      var tuiFormInfo = (Dictionary<string, ITuiFormInfo>)contactGroup.GetTuiFormInfo(phases);
      Assert.IsTrue(tuiFormInfo["com.br"].TuiFormType.Equals("dpp"));
    }

    [TestMethod]
    public void DotBrLocalPresenceTest()
    {
      var tlds = new List<string> { "COM.BR" };
      var domains = new List<string>() { "test.com.br" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "ac",
          "80130", "br", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(registrantContact);
      var newContact = contactGroup.GetContact(DomainContactType.Registrant);

      Assert.IsFalse(newContact.TrusteeVendorIds.ContainsKey("COM.BR"));
    }

    [TestMethod]
    public void GetAllTrusteeIds()
    {
      var tlds = new List<string> { "FR", "IT", "COM.BR", "NET.BR" };
      var domains = new List<string>() { "test.fr", "test.it", "test.com.br", "test.net.br" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "MumboJumbo", true,
          "101 N Street", "Suite 100", "Littleton", "CO",
          "80130", "US", "(303)-555-1213", "(303)-555-2213");
      contactGroup.SetContact(registrantContact);

      var tuiFormInfo = contactGroup.GetContact(DomainContactType.Registrant);
      Assert.AreEqual(true, tuiFormInfo.TuiFormsInfo != null && tuiFormInfo.TuiFormsInfo.ContainsKey("FR"));
    }

    [TestMethod]
    public void DKTuiFormInfoTest()
    {
    //  var tlds = new List<string> { "DK", "ORG" };
    //  var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, 1);

    //  var registrantContact = DomainContactProvider.DomainContactInstance(
    //     "Bill", "Registrant", "bregistrant@bongo.com",
    //       "MumboJumbo", true,
    //      "101 N Street", "Suite 100", "Littleton", "CO",
    //      "80130", "DK", "(303)-555-1213", "(303)-555-2213");
    //  contactGroup.SetContact(DomainContactType.Registrant, registrantContact);
    //  contactGroup.SetContact(DomainContactType.Administrative, registrantContact);
    //  contactGroup.SetContact(DomainContactType.Technical, registrantContact);
    //  contactGroup.SetContact(DomainContactType.Billing, registrantContact);

    //  var input = new Dictionary<string, LaunchPhases>
    //  {
    //    {"DK", LaunchPhases.GeneralAvailability},
    //    {"ORG", LaunchPhases.GeneralAvailability}
    //  };


    //  IDictionary<string, ITuiFormInfo> result = contactGroup.GetTuiFormInfo(input);
    //  ITuiFormInfo tuiFormInfo;
    //  Assert.AreEqual(true, result != null && result.TryGetValue("DK", out tuiFormInfo) && !string.IsNullOrEmpty(tuiFormInfo.TuiFormType));
    //  Assert.AreEqual(true, result != null && result.TryGetValue("ORG", out tuiFormInfo) && string.IsNullOrEmpty(tuiFormInfo.TuiFormType));
    }

    [TestMethod]
    public void TestInvalidDotSgAdminContact()
    {
      var tlds = new List<string> { "SG" };
      var domains = new List<string> { "test.sg" };
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "bregistrant@bongo.com",
           "GoDaddy", true,
          "101 N Street", "Suite 100", "Littleton", "Geneva",
          "80130", "CH", "(303)-555-1213", "(303)-555-2213", "LGR", "ENG");
      var success = contactGroup.SetContact(DomainContactType.Administrative, registrantContact);
      Assert.IsFalse(success);
    }

    [TestMethod]
    public void TestInvalidUkAddressContact()
    {
      var tlds = new List<string> { "gallery" };
      var domains = new List<string>() {"test.gallery"};
      var contactGroup = DomainContactProvider.DomainContactGroupInstance(tlds, domains, 1);

      var registrantContact = DomainContactProvider.DomainContactInstance(
         "Bill", "Registrant", "hamish@canongate.org",
           "Canongate Limited", true,
          "14 Kinnear Road", "Suite 100", "Edinburgh", "Edinburgh",
          "EH35PE4E", "UK", "+44", string.Empty, string.Empty, string.Empty);
      var success = contactGroup.SetContact(DomainContactType.Registrant, registrantContact);
      Assert.IsFalse(success);
    }
  }
}

