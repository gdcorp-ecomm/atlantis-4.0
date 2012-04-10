using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using Atlantis.Framework.FastballLogImpressions.Interface;

namespace Atlantis.Framework.FastballLogImpressions.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class FastballLogImpressionsTests
  {
    public FastballLogImpressionsTests()
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
      List<FastballTrafficEvent> eventList = new List<FastballTrafficEvent>();
      FastballTrafficEvent newEvent = new FastballTrafficEvent
                                          {
                                            EventDate = DateTime.Now,
                                            EventOfferId = "34554",
                                            PageSequence = "1",
                                            EventType = FastballTrafficEventType.Impression
                                          };

      
      eventList.Add(newEvent);

      newEvent = new FastballTrafficEvent
      {
        EventDate = DateTime.Now,
        EventOfferId = "34554",
        PageSequence = "1",
        EventType = FastballTrafficEventType.Impression
      };

      eventList.Add(newEvent);

      newEvent = new FastballTrafficEvent
      {
        EventDate = DateTime.Now,
        EventOfferId = "34554",
        PageSequence = "1",
        EventType = FastballTrafficEventType.Click
      };

      eventList.Add(newEvent);

      FastballLogImpressionsRequestData request = new FastballLogImpressionsRequestData(
        "860427", "http://yuck.com", string.Empty, "TestPathGuid", 1, eventList, Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString());
       
      
      FastballLogImpressionsResponseData response = (FastballLogImpressionsResponseData)Engine.Engine.ProcessRequest(request, 283);      
      Assert.IsTrue(response.IsSuccess);
    }
   
  }
}
