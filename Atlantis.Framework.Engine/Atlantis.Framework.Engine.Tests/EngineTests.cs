using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Engine.Tests.MockTriplet;
using Atlantis.Framework.Interface;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Atlantis.Framework.Engine.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class EngineTests
  {
    public EngineTests()
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
    public void ConfigElementWithCustomValues()
    {
      ConfigTestRequestData request = new ConfigTestRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9999);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ConfigElementWithoutCustomValues()
    {
      ConfigTestRequestData request = new ConfigTestRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
      ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9998);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void LogException()
    {
      AtlantisException ex = new AtlantisException("EngineTests.LogException", "911", "Test log message only.", string.Empty, null, null);
      Engine.LogAtlantisException(ex);

      Assert.AreEqual(LoggingStatusType.WorkingNormally, Engine.LoggingStatus);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void EngineStats()
    {
      for (int i = 0; i < 500; i++)
      {
        try
        {
          ConfigTestRequestData request = new ConfigTestRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
          ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9997);
        }
        catch { }
      }

      ConfigElement config;
      if (Engine.TryGetConfigElement(9997, out config))
      {
        Console.WriteLine(config.ProgID);
        Console.WriteLine("File Version = " + config.AssemblyFileVersion);
        Console.WriteLine("Description = " + config.AssemblyDescription);
        Console.WriteLine("Succeeded: " + config.Stats.Succeeded.ToString());
        Console.WriteLine("Failed: " + config.Stats.Failed.ToString());

        TimeSpan averageSuccessTime = config.Stats.CalculateAverageSuccessTime();
        Console.WriteLine("Success Avg ms: " + averageSuccessTime.TotalMilliseconds.ToString());

        TimeSpan averageFailTime = config.Stats.CalculateAvarageFailTime();
        Console.WriteLine("Fail Avg ms: " + averageFailTime.TotalMilliseconds.ToString());
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MonitorData()
    {
      for (int i = 0; i < 500; i++)
      {
        try
        {
          ConfigTestRequestData request = new ConfigTestRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
          ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9997);
        }
        catch { }
      }

      XDocument stats = Monitor.MonitorData.GetMonitorData(Monitor.MonitorDataTypes.Stats);
      Assert.IsNotNull(stats);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void LogExceptionOverriddenLogger()
    {
      TestLogger testLogger = new TestLogger();
      IErrorLogger oldLogger = EngineLogging.EngineLogger;

      try
      {
        EngineLogging.EngineLogger = testLogger;

        AtlantisException ex = new AtlantisException("EngineTests.LogException", "911", "Test log message only.", string.Empty, null, null);
        Engine.LogAtlantisException(ex);

        Assert.AreEqual(LoggingStatusType.WorkingNormally, Engine.LoggingStatus);
        Assert.IsTrue(testLogger.IGotLogged);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    private class TestLogger : IErrorLogger
    {
      public bool IGotLogged { get; set; }

      public TestLogger()
      {
        IGotLogged = false;
      }

      public void LogAtlantisException(AtlantisException atlantisException)
      {
        IGotLogged = atlantisException != null;
      }
    }
  }
}
