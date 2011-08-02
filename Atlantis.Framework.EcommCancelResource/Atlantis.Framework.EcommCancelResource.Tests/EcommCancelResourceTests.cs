using System.Diagnostics;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.EcommCancelResource.Interface;

namespace Atlantis.Framework.EcommCancelResource.Tests
{
  [TestClass]
  public class GetEcommCancelResourceTests
  {

    private const string _shopperId = "842749";
    private const int _ecommCancelResourceRequestType = 165;


    public GetEcommCancelResourceTests()
    {
    }

    public TestContext TestContext { get; set; }

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
    public void EcommCancelResourceFailTest()
    {
      EcommCancelResourceRequestData request = new EcommCancelResourceRequestData("32675"
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 496322
         , "socdomain"
         , "immediate"
         , "Cust-32675"
         , "172.16.45.116");

      EcommCancelResourceResponseData response = (EcommCancelResourceResponseData)Engine.Engine.ProcessRequest(request, _ecommCancelResourceRequestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EcommCancelResourceSucceedTest()
    {
      System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
      string ipAddress = hostEntry.AddressList[0].ToString();

      // ResourceId and ResourceType need to be updated each time to ensure product exists.

      EcommCancelResourceRequestData request = new EcommCancelResourceRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 321318
         , "photoalbum"
         , "immediate"
         , "Cust-842749"
         , ipAddress);

      EcommCancelResourceResponseData response = (EcommCancelResourceResponseData)Engine.Engine.ProcessRequest(request, _ecommCancelResourceRequestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EcommCancelResourceXmlInputTest()
    {
      XDocument xDoc = new XDocument(
        new XElement("cancellation",
          new XAttribute("shopperid", "842749"),
          new XAttribute("cancel_by", "Cust-842749"),
          new XAttribute("UserIP", "127.0.0.1"),
        new XElement("cancel",
          new XAttribute("type", "pending"),
          new XAttribute("id", "Hosting:307044")),
        new XElement("cancel",
          new XAttribute("type", "pending"),
          new XAttribute("id", "Domain:342117")
      )));

      EcommCancelResourceRequestData request = new EcommCancelResourceRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , xDoc.ToString());

      EcommCancelResourceResponseData response = (EcommCancelResourceResponseData)Engine.Engine.ProcessRequest(request, _ecommCancelResourceRequestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);

    }
  }
}
