using System;
using Atlantis.Framework.EcommCreditCardReqs.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommCreditCardReqs.Test
{


  /// <summary>
  ///This is a test class for EcommCreditCardReqsRequestTest and is intended
  ///to contain all EcommCreditCardReqsRequestTest Unit Tests
  ///</summary>
  [TestClass()]
  public class EcommCreditCardReqsRequestTest
  {
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
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CheckCreditCard()
    {
      EcommCreditCardReqsRequestData request = new EcommCreditCardReqsRequestData("850774", string.Empty, string.Empty,
                                                                                  string.Empty,
                                                                                  0, "4173180000000003", 0, 1, String.Empty, String.Empty);
      EcommCreditCardReqsResponseData response = (EcommCreditCardReqsResponseData)Engine.Engine.ProcessRequest(request, 246);
      Assert.IsTrue(response.IsSuccess);
      if (response.IsSuccess)
      {
        System.Xml.XmlDocument oDoc = new System.Xml.XmlDocument();
        oDoc.LoadXml(response.ToXML());
        //<CardRequirements card_specific_gateway_id=\"13\" require_cvv=\"1\" require_3ds=\"0\"/>\n"
        Assert.IsTrue(!String.IsNullOrEmpty(oDoc.ChildNodes[0].Attributes["require_cvv"].Value));
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CheckCreditCardProfile()
    {
      EcommCreditCardReqsRequestData request = new EcommCreditCardReqsRequestData("859012", string.Empty, string.Empty,
                                                                                  string.Empty,
                                                                                  0, string.Empty, 65764, 1, "MXN", "mx");
      EcommCreditCardReqsResponseData response = (EcommCreditCardReqsResponseData)Engine.Engine.ProcessRequest(request, 246);
      Assert.IsTrue(response != null);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void CheckCreditCardFailure()
    {
      EcommCreditCardReqsRequestData request = new EcommCreditCardReqsRequestData("850774", string.Empty, string.Empty,
                                                                                  string.Empty,
                                                                                  0, "555", 0, 1, String.Empty, String.Empty);
      EcommCreditCardReqsResponseData response = (EcommCreditCardReqsResponseData)Engine.Engine.ProcessRequest(request, 246);
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
