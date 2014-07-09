using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using Atlantis.Framework.DomainCheck.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainCheck.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DomainCheckTests
  {
    public DomainCheckTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainCheck.Impl.dll")]
    public void DomainCheckBasicTest()
    {
      DomainToCheck domain = new DomainToCheck("miccobluered7.com", false);
      DomainCheckRequestData request = new DomainCheckRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        domain, 1, "127.0.0.1", "UnitTest");
      request.WaitTime = TimeSpan.FromSeconds(5);

      DomainCheckResponseData response = (DomainCheckResponseData)Engine.Engine.ProcessRequest(request, 16);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainCheck.Impl.dll")]
    public void DomainCheckTimeoutTest()
    {
      List<DomainToCheck> domains = new List<DomainToCheck>();
      domains.Add(new DomainToCheck("micco.com", false));
      domains.Add(new DomainToCheck("micco1.com", false));
      domains.Add(new DomainToCheck("micco2.com", false));
      domains.Add(new DomainToCheck("micco3.com", false));
      domains.Add(new DomainToCheck("micco4.com", false));

      DomainCheckRequestData request = new DomainCheckRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        domains, 1, "127.0.0.1", "UnitTest");
      request.WaitTime = TimeSpan.FromSeconds(5);
      request.RequestTimeout = TimeSpan.FromMilliseconds(1);

      DomainCheckResponseData response = (DomainCheckResponseData)Engine.Engine.ProcessRequest(request, 16);
      Assert.IsFalse(response.IsSuccess);
      Assert.AreEqual(response.ServiceExceptionStatus, WebExceptionStatus.Timeout);

    }

    private volatile bool _asyncSearchComplete = false;
    private DomainCheckResponseData _asyncResponse = null;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainCheck.Impl.dll")]
    public void DomainCheckAsyncTest()
    {
      _asyncResponse = null;

      List<DomainToCheck> domains = new List<DomainToCheck>();
      domains.Add(new DomainToCheck("stair.com", false));
      domains.Add(new DomainToCheck("stair1.com", false));
      domains.Add(new DomainToCheck("stair2.com", false));
      domains.Add(new DomainToCheck("stair3.com", false));
      domains.Add(new DomainToCheck("stair4.com", false));

      DomainCheckRequestData request = new DomainCheckRequestData(
        "70707070", string.Empty, string.Empty, string.Empty, 0,
        domains, 1, "127.0.0.1", "UnitTest");
      request.WaitTime = TimeSpan.FromSeconds(5);
      request.RequestTimeout = TimeSpan.FromMilliseconds(1);

      IAsyncResult asyncResult = Engine.Engine.BeginProcessRequest(request, 21, EndDomainCheckAsyncTest, null);
      while (!_asyncSearchComplete)
      {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
      }

      Assert.IsNotNull(_asyncResponse);
      Assert.IsTrue(_asyncResponse.IsSuccess);
    }

    private void EndDomainCheckAsyncTest(IAsyncResult result)
    {
      _asyncResponse = Engine.Engine.EndProcessRequest(result) as DomainCheckResponseData;
      _asyncSearchComplete = true;
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainCheck.Impl.dll")]
    public void DomainCheckShopperIdAttributeTest()
    {
      DomainToCheck domain = new DomainToCheck("miccobluered8.com", false);
      DomainCheckRequestData request = new DomainCheckRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0,
        domain, 1, "127.0.0.1", "UnitTest");
      request.WaitTime = TimeSpan.FromSeconds(5);

      DomainCheckResponseData response = (DomainCheckResponseData)Engine.Engine.ProcessRequest(request, 16);

      var doc = XDocument.Parse(request.ToXML());

      Assert.IsTrue(doc.Root.Attribute("shopperID").Value == "832652");
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainCheck.Impl.dll")]
    public void DomainCheckExactTypedTest()
    {
      var shopperID = string.Empty;// "12345678";
      const string domainname = "shaun1.stair";
      const bool wastyped = true;
      const string typedDomainName = "shaun2.stair"; //string.Empty;
      const string tldChoice = "com";
      const bool wasTldSelected = false;
      const bool specialCharsRemoved = false;
      var splitValue = string.Empty; //"54";

      var domain = new DomainToCheck(domainname, wastyped, typedDomainName, tldChoice, wasTldSelected, specialCharsRemoved, splitValue);
      
      var request = new DomainCheckRequestData(shopperID, string.Empty, string.Empty, string.Empty, 6, domain, 1, "127.0.0.1", "UnitTest:DomainCheckExactTypedTest");

      var response = (DomainCheckResponseData)Engine.Engine.ProcessRequest(request, 16);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainCheck.Impl.dll")]
    public void AsyncDomainCheckExactTypedTest()
    {
      const string shopperID = "12345678";
      const string domainname = "shaun.stair";
      const bool wastyped = true;
      const string typedDomainName = "shaun.stair"; //string.Empty;
      const string tldChoice = "com";
      const bool wasTldSelected = false;
      const bool specialCharsRemoved = true;
      const string splitValue = "54";

      var domains = new List<DomainToCheck>
                      {
                        new DomainToCheck(domainname, wastyped, typedDomainName, tldChoice, wasTldSelected, specialCharsRemoved, splitValue),
                        new DomainToCheck("shaunstair.com", false),
                        new DomainToCheck("shaun3.stair", false),
                        new DomainToCheck("shaun4.stair", false),
                        new DomainToCheck("shaunstair.net", false)
                      };


      //var request = new DomainCheckRequestData(shopperID, "127.0.0.1", string.Empty, "", 1, 1, "127.0.0.1",
      //                                         "UnitTest:DomainCheckExactTypedTest")
      //                {
      //                  WaitTime = TimeSpan.FromMilliseconds(2500),
      //                  RegistrarID = "1",
      //                  RequestTimeout = TimeSpan.FromMilliseconds(5000),
      //                  TypedDomainName = domainname,
      //                  WasTLDSelected = wasTldSelected,
      //                  SpecialCharsRemoved = specialCharsRemoved,
      //                  SplitValue = splitValue,
      //                  TldChoice = tldChoice
      //                };

      //request.AddDomain(new DomainToCheck(domainname, wastyped, typedDomainName, tldChoice, wasTldSelected, specialCharsRemoved, splitValue));

      //request.AddDomain(new DomainToCheck("shaunstair.com", false));
      //request.AddDomain(new DomainToCheck("shaun2.stair", false));
      //request.AddDomain(new DomainToCheck("shaun3.stair", false));
      //request.AddDomain(new DomainToCheck("shaunstair.net", false));


      var request = new DomainCheckRequestData(shopperID, string.Empty, string.Empty, string.Empty, 6, domains, 1, "127.0.0.1", "UnitTest:DomainCheckExactTypedTest");

      var requestXml = request.ToXML();
      var response = (DomainCheckResponseData)Engine.Engine.ProcessRequest(request, 16);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(requestXml.Contains("typedDomainName"));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DomainCheck.Impl.dll")]
    public void DomainFindCheckTest()
    {
      const string domainname = "доменноеимя.com";
      const string shopperId = "12345678";
      const bool wastyped = true;
      const string typedDomainName = "доменноеимя.com";
      const string tldChoice = "com";
      const bool wasTldSelected = false;
      const bool specialCharsRemoved = true;
      const string splitValue = "54";
      var domains = new List<DomainToCheck>
                      {
                        new DomainToCheck(domainname, wastyped, typedDomainName, tldChoice, wasTldSelected, specialCharsRemoved, splitValue),
                        new DomainToCheck("latinchardomainname1.com", false),
                        new DomainToCheck("latinchardomainname2.com", false),
                        new DomainToCheck("latinchardomainname3.com", false),
                        new DomainToCheck("ещёоднодоменноеимя.net", false)
                      };
      var request = new DomainCheckRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, domains, 1, "127.0.0.1", "UnitTest-DomainFindCheckTest");
      var requestXml = request.ToXML();
      Assert.IsTrue(!string.IsNullOrEmpty(requestXml));
      var response = (DomainCheckResponseData)Engine.Engine.ProcessRequest(request, 16);
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.FirstDomain.Key.Equals("доменноеимя.com"));
      Assert.IsTrue(response.FirstDomain.Value.WasTyped);
      Assert.IsTrue(response.FirstDomain.Value.InternalTierId < 0);
      Assert.IsTrue(response.FirstDomain.Value.SyntaxCode > -1);
      Assert.IsTrue(response.FirstDomain.Value.AvailableCode > -1);
      Assert.IsTrue(response.FirstDomain.Value.PunyCode.Equals("xn--d1acamrdeafe4q.com"));
      Assert.IsTrue(response.FirstDomain.Value.IdnScript.Equals("KUR"));
      Assert.IsTrue(response.FirstDomain.Value.LanguageId.Equals("64"));
    }
  }
}
