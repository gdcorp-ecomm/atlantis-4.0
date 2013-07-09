using Atlantis.Framework.Engine.Tests.MockTriplet;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using System.Linq;

namespace Atlantis.Framework.Engine.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  public class EngineTests
  {
    IErrorLogger _defaultEngineLogger;
    EngineTestsLogger _engineTestsLogger;

    public EngineTests()
    {
      _engineTestsLogger = new EngineTestsLogger();
    }

    [TestInitialize]
    public void Initialize()
    {
      _defaultEngineLogger = EngineLogging.EngineLogger;
      EngineLogging.EngineLogger = _engineTestsLogger;
    }

    [TestCleanup]
    public void Cleanup()
    {
      EngineLogging.EngineLogger = _defaultEngineLogger;
    }

    [TestMethod]
    public void ConfigElementWithCustomValues()
    {
      ConfigTestRequestData request = new ConfigTestRequestData();
      ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9999);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void ConfigElementWithoutCustomValues()
    {
      ConfigTestRequestData request = new ConfigTestRequestData();
      ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9998);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void LogException()
    {
      AtlantisException ex = new AtlantisException("EngineTests.LogException", "911", "Test log message only.", string.Empty, null, null);
      Engine.LogAtlantisException(ex);

      Assert.AreEqual(LoggingStatusType.WorkingNormally, Engine.LoggingStatus);
    }

    [TestMethod]
    public void EngineStats()
    {
      for (int i = 0; i < 500; i++)
      {
        try
        {
          ConfigTestRequestData request = new ConfigTestRequestData();
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

        TimeSpan averageFailTime = config.Stats.CalculateAverageFailTime();
        Console.WriteLine("Fail Avg ms: " + averageFailTime.TotalMilliseconds.ToString());
      }
    }

    [TestMethod]
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

    [TestMethod]
    public void LogExceptionBadLogger()
    {
      LoggerWithException badLogger = new LoggerWithException();
      IErrorLogger oldLogger = EngineLogging.EngineLogger;

      try
      {
        EngineLogging.EngineLogger = badLogger;

        AtlantisException ex = new AtlantisException("EngineTests.LogException", "911", "Test log message only.", string.Empty, null, null);
        Engine.LogAtlantisException(ex);

        Assert.AreEqual(LoggingStatusType.Error, Engine.LoggingStatus);
        Assert.IsNotNull(Engine.LastLoggingError);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void LogExceptionNullLogger()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;

      try
      {
        EngineLogging.EngineLogger = null;

        AtlantisException ex = new AtlantisException("EngineTests.LogException", "911", "Test log message only.", string.Empty, null, null);
        Engine.LogAtlantisException(ex);

        Assert.AreEqual(LoggingStatusType.NullLogger, Engine.LoggingStatus);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void LogExceptionCustomLoggerGeneric()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;

      try
      {
        EngineLogging.EngineLogger = null;

        AtlantisException ex = new AtlantisException("EngineTests.LogException", "911", "Test log message only.", string.Empty, null, null);
        Engine.LogAtlantisException<TestLogger>(ex);

        Assert.AreEqual(LoggingStatusType.WorkingNormally, Engine.LoggingStatus);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void LogExceptionBadConstructorGeneric()
    {
      AtlantisException ex = new AtlantisException("EngineTests.LogException", "911", "Test log message only.", string.Empty, null, null);
      Engine.LogAtlantisException(ex);
      Assert.AreEqual(LoggingStatusType.WorkingNormally, Engine.LoggingStatus);

      Engine.LogAtlantisException<ErrorLoggerWithBadConstructor>(ex);
      Assert.AreEqual(LoggingStatusType.Error, Engine.LoggingStatus);
      Assert.AreEqual(typeof(ApplicationException), Engine.LastLoggingError.InnerException.GetType());
    }

    [TestMethod]
    public void DefaultEngineLogger()
    {
      _engineTestsLogger.ClearExceptions();
      AtlantisException ex = new AtlantisException("EngineTests.LogException", "911", "Test log message only.", string.Empty, null, null);
      Engine.LogAtlantisException(ex, _defaultEngineLogger);
      Assert.AreEqual(0, _engineTestsLogger.ExceptionCount);
    }

    [TestMethod]
    public void LogExceptionCustomLoggerPreMade()
    {
      TestLogger testLogger = new TestLogger();
      IErrorLogger oldLogger = EngineLogging.EngineLogger;

      try
      {
        EngineLogging.EngineLogger = null;

        AtlantisException ex = new AtlantisException("EngineTests.LogException", "911", "Test log message only.", string.Empty, null, null);
        Engine.LogAtlantisException(ex, testLogger);

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

    private class LoggerWithException : IErrorLogger
    {
      public void LogAtlantisException(AtlantisException atlantisException)
      {
        throw new ApplicationException();
      }
    }

    [TestMethod]
    public void LogExceptionWithoutDescription()
    {
      Exception rootCause = new Exception("Root Cause");
      var requestData = new ConfigTestRequestData();
      AtlantisException atlantisException = new AtlantisException(requestData, string.Empty, string.Empty, string.Empty, rootCause);
      Engine.LogAtlantisException(atlantisException);

      Assert.AreEqual(LoggingStatusType.WorkingNormally, Engine.LoggingStatus);
    }

    [TestMethod]
    public void EngineStatsReset()
    {
      for (int i = 0; i < 500; i++)
      {
        try
        {
          ConfigTestRequestData request = new ConfigTestRequestData();
          ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9997);
        }
        catch { }
      }

      ConfigElement config;
      if (Engine.TryGetConfigElement(9997, out config))
      {
        ConfigElementStats existingStats = config.ResetStats();
        Assert.AreNotEqual(0, existingStats.Succeeded);
        Assert.AreEqual(0, config.Stats.Succeeded);
      }
    }

    [TestMethod]
    public void ReloadConfig()
    {
      ConfigTestRequestData request = new ConfigTestRequestData();
      ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9999);

      Engine.ReloadConfig();

      request = new ConfigTestRequestData();
      response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9999);
    }

    [TestMethod]
    public void AsyncRequest()
    {
      TestTripletRequestData request = new TestTripletRequestData("blue");
      IAsyncResult resultInProgress = Engine.BeginProcessRequest(request, 9990, null, null);

      Stopwatch timer = Stopwatch.StartNew();
      bool done = false;
      while (!done)
      {
        if (resultInProgress.IsCompleted)
        {
          done = true;
        }

        if (timer.Elapsed.TotalSeconds > 2)
        {
          throw new TimeoutException();
        }
      }

      TestTripletResponseData response = (TestTripletResponseData)Engine.EndProcessRequest(resultInProgress);
      Assert.AreEqual("blue", response.ResultData);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void AsyncRequestExceptionBeforeAsyncStart()
    {
      TestTripletRequestData request = new TestTripletRequestData("beginerror");
      Engine.BeginProcessRequest(request, 9990, null, null);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void AsyncRequestAtlantisExceptionBeforeAsyncStart()
    {
      TestTripletRequestData request = new TestTripletRequestData("beginerroratlantis");
      Engine.BeginProcessRequest(request, 9990, null, null);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void AsyncRequestExceptionDuringAsync()
    {
      TestTripletRequestData request = new TestTripletRequestData("duringerror");
      IAsyncResult resultInProgress = Engine.BeginProcessRequest(request, 9990, null, null);

      Stopwatch timer = Stopwatch.StartNew();
      bool done = false;
      while (!done)
      {
        if (resultInProgress.IsCompleted)
        {
          done = true;
        }

        if (timer.Elapsed.TotalSeconds > 10)
        {
          throw new TimeoutException();
        }
      }

      Engine.EndProcessRequest(resultInProgress);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void AsyncRequestAtlantisExceptionDuringResponse()
    {
      TestTripletRequestData request = new TestTripletRequestData("enderroratlantis");
      IAsyncResult resultInProgress = Engine.BeginProcessRequest(request, 9990, null, null);

      Stopwatch timer = Stopwatch.StartNew();
      bool done = false;
      while (!done)
      {
        if (resultInProgress.IsCompleted)
        {
          done = true;
        }

        if (timer.Elapsed.TotalSeconds > 10)
        {
          throw new TimeoutException();
        }
      }

      Engine.EndProcessRequest(resultInProgress);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void AsyncRequestExceptionOnResponse()
    {
      TestTripletRequestData request = new TestTripletRequestData("responseerror");
      IAsyncResult resultInProgress = Engine.BeginProcessRequest(request, 9990, null, null);

      Stopwatch timer = Stopwatch.StartNew();
      bool done = false;
      while (!done)
      {
        if (resultInProgress.IsCompleted)
        {
          done = true;
        }

        if (timer.Elapsed.TotalSeconds > 10)
        {
          throw new TimeoutException();
        }
      }

      Engine.EndProcessRequest(resultInProgress);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void AsyncRequestThreadAbort()
    {
      TestTripletRequestData request = new TestTripletRequestData("threadabort");
      IAsyncResult resultInProgress = Engine.BeginProcessRequest(request, 9990, null, null);

      Stopwatch timer = Stopwatch.StartNew();
      bool done = false;
      while (!done)
      {
        if (resultInProgress.IsCompleted)
        {
          done = true;
        }

        if (timer.Elapsed.TotalSeconds > 10)
        {
          throw new TimeoutException();
        }
      }

      Engine.EndProcessRequest(resultInProgress);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void AsyncRequestInvalidState()
    {
      TestTripletRequestData request = new TestTripletRequestData("invalidstate");
      IAsyncResult resultInProgress = Engine.BeginProcessRequest(request, 9990, null, null);

      Stopwatch timer = Stopwatch.StartNew();
      bool done = false;
      while (!done)
      {
        if (resultInProgress.IsCompleted)
        {
          done = true;
        }

        if (timer.Elapsed.TotalSeconds > 10)
        {
          throw new TimeoutException();
        }
      }

      Engine.EndProcessRequest(resultInProgress);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void SyncRequestExceptionOnResponse()
    {
      TestTripletRequestData request = new TestTripletRequestData("responseerror");
      Engine.ProcessRequest(request, 9991);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void SyncRequestExceptionDuringRequest()
    {
      TestTripletRequestData request = new TestTripletRequestData("beginerror");
      Engine.ProcessRequest(request, 9991);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void SyncRequestAtlantisExceptionDuringRequest()
    {
      TestTripletRequestData request = new TestTripletRequestData("beginerroratlantis");
      Engine.ProcessRequest(request, 9991);
    }

    int _requestType = -1;

    [TestMethod]
    public void SyncRequestWithDelegate()
    {
      _requestType = -1;
      Engine.OnRequestCompleted += Engine_OnRequestCompleted;

      try
      {
        TestTripletRequestData request = new TestTripletRequestData("green");
        var response = (TestTripletResponseData)Engine.ProcessRequest(request, 9991);

        Assert.AreEqual(9991, _requestType);
      }
      finally
      {
        Engine.OnRequestCompleted -= Engine_OnRequestCompleted;
      }
    }

    void Engine_OnRequestCompleted(ICompletedRequest completedRequest)
    {
      TimeSpan time = completedRequest.ElapsedTime;
      Assert.IsNotNull(completedRequest.RequestData);
      Assert.IsFalse(string.IsNullOrEmpty(completedRequest.ToString()));
      _requestType = completedRequest.Config.RequestType;
    }

    [TestMethod]
    public void AsyncRequestWithDelegate()
    {
      _requestType = -1;
      Engine.OnRequestCompleted += Engine_OnRequestCompleted;

      try
      {
        TestTripletRequestData request = new TestTripletRequestData("blue");
        IAsyncResult resultInProgress = Engine.BeginProcessRequest(request, 9990, null, null);

        Stopwatch timer = Stopwatch.StartNew();
        bool done = false;
        while (!done)
        {
          if (resultInProgress.IsCompleted)
          {
            done = true;
          }

          if (timer.Elapsed.TotalSeconds > 30)
          {
            throw new TimeoutException();
          }
        }

        Engine.EndProcessRequest(resultInProgress);

        Assert.AreEqual(9990, _requestType);
      }
      finally
      {
        Engine.OnRequestCompleted -= Engine_OnRequestCompleted;
      }
    }

    bool _exceptionStillHitDelegate = false;

    [TestMethod]
    public void SyncRequestExceptionWithDelegate()
    {
      _exceptionStillHitDelegate = false;
      Engine.OnRequestCompleted += Engine_OnRequestCompletedWithException;

      try
      {
        TestTripletRequestData request = new TestTripletRequestData("beginerror");
        var response = (TestTripletResponseData)Engine.ProcessRequest(request, 9991);
      }
      catch
      {
        Assert.IsTrue(_exceptionStillHitDelegate);
      }
      finally
      {
        Engine.OnRequestCompleted -= Engine_OnRequestCompletedWithException;
      }
    }

    void Engine_OnRequestCompletedWithException(ICompletedRequest completedRequest)
    {
      _exceptionStillHitDelegate = completedRequest.Exception != null;
    }

    [TestMethod]
    public void SyncRequestExceptionWithBadDelegate()
    {
      _engineTestsLogger.ClearExceptions();

      Engine.OnRequestCompleted += Engine_OnRequestCompletedBad;

      try
      {
        TestTripletRequestData request = new TestTripletRequestData("green");
        var response = (TestTripletResponseData)Engine.ProcessRequest(request, 9991);
        Assert.AreEqual(1, _engineTestsLogger.ExceptionCount);
        Assert.IsTrue(_engineTestsLogger.ExceptionsLogged.First().Message.StartsWith("Bad Delegate"));
      }
      finally
      {
        Engine.OnRequestCompleted -= Engine_OnRequestCompletedBad;
      }
    }

    void Engine_OnRequestCompletedBad(ICompletedRequest completedRequest)
    {
      throw new ApplicationException("Bad Delegate");
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void SyncRequestWithNullResponse()
    {
      TestTripletRequestData request = new TestTripletRequestData("null");
      var response = (TestTripletResponseData)Engine.ProcessRequest(request, 9991);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void InvalidRequestType()
    {
      TestTripletRequestData request = new TestTripletRequestData("green");
      var response = (TestTripletResponseData)Engine.ProcessRequest(request, 19991);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void SyncRequestWithDelegateAndNullResponse()
    {
      Engine.OnRequestCompleted += Engine_OnRequestCompletedNullResponse;

      try
      {
        TestTripletRequestData request = new TestTripletRequestData("null");
        var response = (TestTripletResponseData)Engine.ProcessRequest(request, 9991);
      }
      finally
      {
        Engine.OnRequestCompleted -= Engine_OnRequestCompletedNullResponse;
      }
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void AsyncRequestWithDelegateAndNullResponse()
    {
      _requestType = -1;
      Engine.OnRequestCompleted += Engine_OnRequestCompletedNullResponse;
      
      try
      {
        TestTripletRequestData request = new TestTripletRequestData("null");
        IAsyncResult resultInProgress = Engine.BeginProcessRequest(request, 9990, null, null);

        Stopwatch timer = Stopwatch.StartNew();
        bool done = false;
        while (!done)
        {
          if (resultInProgress.IsCompleted)
          {
            done = true;
          }

          if (timer.Elapsed.TotalSeconds > 30)
          {
            throw new TimeoutException();
          }
        }

        Engine.EndProcessRequest(resultInProgress);

        Assert.AreEqual(9990, _requestType);
      }
      finally
      {
        Engine.OnRequestCompleted -= Engine_OnRequestCompletedNullResponse;
      }
    }

    void Engine_OnRequestCompletedNullResponse(ICompletedRequest completedRequest)
    {
      Assert.IsNotNull(completedRequest.Exception);
      string value = completedRequest.ToString();
      Assert.IsNull(completedRequest.ResponseData);
    }

  }
}
