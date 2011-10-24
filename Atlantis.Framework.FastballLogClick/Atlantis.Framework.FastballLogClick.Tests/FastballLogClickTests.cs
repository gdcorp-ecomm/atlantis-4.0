using System;
using System.IO;
using System.Reflection;
using Atlantis.Framework.FastballLogClick.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FastballLogClick.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class FastballLogClickTests
  {
    public FastballLogClickTests()
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

    private string LoadSampleXml(string filename)
    {
      string path = Assembly.GetExecutingAssembly().Location;
      string fullpath = Path.Combine(Path.GetDirectoryName(path), filename);
      string sampleXml = File.ReadAllText(fullpath);
      return sampleXml;
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
    public void BasicRequestXml()
    {      
      FastballLogClickRequestData request = new FastballLogClickRequestData(
        "860427", "http://yuck.com", string.Empty, "TestPathGuid", 0, 1, 40, 30559, Guid.NewGuid().ToString() , null);
      FastballLogClickResponseData response = (FastballLogClickResponseData)Engine.Engine.ProcessRequest(request, 255);      
      Assert.IsTrue(response.IsSuccess);      		
    }

    [TestMethod]
    public void InvalidRequestXml()
    {
      FastballLogClickRequestData request = new FastballLogClickRequestData(
        "860427", "http://yuck.com", string.Empty, "TestPathGuid", 0, 1, 40, 30559, "InvalidGuid", null);
      FastballLogClickResponseData response = (FastballLogClickResponseData)Engine.Engine.ProcessRequest(request, 255);
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
