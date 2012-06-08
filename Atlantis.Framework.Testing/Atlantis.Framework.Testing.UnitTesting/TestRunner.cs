using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.UnitTesting.BaseClasses;
using Atlantis.Framework.Testing.UnitTesting.Exceptions;

namespace Atlantis.Framework.Testing.UnitTesting
{

  public sealed class TestRunner
  {

    #region Properties

    public bool RunDestructiveTests { get; set; }

    private TestResultData _testData;
    public TestResultData TestData
    {
      get
      {
        if (_testData == null)
        {
          _testData = new TestResultData();
        }
        return _testData;
      }
    }

    private ISiteContext _siteContext;

    private ISiteContext SiteContext
    {
      get
      {
        if (_siteContext == null)
        {
          _siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        }
        return _siteContext;
      }
    }

    private UnitTestBase _currentTestClass = null;
    internal UnitTestBase CurrentTestClass
    {
      get { return _currentTestClass; }
    }

    #endregion


    #region public

    public void ExecuteTests(string classToTest)
    {
      try
      {
        _currentTestClass = Activator.CreateInstance(Assembly.Load("App_Code").GetType(classToTest)) as UnitTestBase;
        MethodInfo fixtureSetup;
        MethodInfo fixtureTeardown;
        MethodInfo testSetup;
        MethodInfo testTeardown;

        if (CurrentTestClass != null)
          CurrentTestClass.Log += new UnitTestBase.LogData(AddLogData);

        AddLogData(this, new UnitTestLogDataEventArgs("Test Runner", String.Format("Initialized at {0}", DateTime.Now)));

        var tests = GetTests(out fixtureSetup, out fixtureTeardown, out testSetup, out testTeardown);
        if (tests.Count > 0)
        {
          RunTestFixtureSetup(fixtureSetup);

          foreach (var test in tests)
          {

            try
            {
              RunTestSetup(testSetup);

              RunTest(test);

              RunTestTeardown(testTeardown);

            }
            catch (Exception ex)
            {
              AddTestResult(false, test.Name, "The test suite runner encountered an error. " + ex.Message);
            }

          }

          RunTestFixtureTeardown(fixtureTeardown);

        }
      }
      catch (Exception e)
      {
        AddTestResult(false, e.GetType().ToString(), "The test suite runner encountered an error. " + e.Message);
      }

      AddLogData(this, new UnitTestLogDataEventArgs("Test Runner", String.Format("Completed at {0}", DateTime.Now)));

    }

    #endregion


    #region private

    private void AddTestResult(bool? successValue, string testName, string message)
    {
      if (TestData.TestResults == null)
      {
        TestData.TestResults = new List<TestResultBase>();
      }

      TestData.TestResults.Add(new TestResultBase(successValue, testName, message));

    }

    private void AddLogData(object sender, UnitTestLogDataEventArgs e)
    {
      if (TestData.ExtendedLogData.ContainsKey(e.MethodName))
      {
        TestData.ExtendedLogData[e.MethodName].Add(e.Data);
      }
      else
      {
        TestData.ExtendedLogData.Add(e.MethodName, new TestExtendedLogDataEntries() { e.Data });
      }

    }

    private List<MethodInfo> GetTests(out MethodInfo fixtureSetup, out MethodInfo fixtureTeardown, out MethodInfo testSetup, out MethodInfo testTeardown)
    {
      var tests = new List<MethodInfo>();
      var testType = CurrentTestClass.GetType();

      fixtureSetup = null;
      fixtureTeardown = null;
      testSetup = null;
      testTeardown = null;

      var fixtureAttr = (TestFixtureAttribute)Attribute.GetCustomAttribute(testType, typeof(TestFixtureAttribute));
      if (fixtureAttr == null)
      {
        throw new InvalidTestFixtureException("Expected [TestFixture] attribute to be present on testable class.");
      }
      else
      {
        RunDestructiveTests = fixtureAttr.AllowDestructiveTests;
      }



      foreach (MethodInfo mi in testType.GetMethods())
      {
        var fixtureSetupAttr = (TestFixtureSetupAttribute)Attribute.GetCustomAttribute(mi, typeof(TestFixtureSetupAttribute));
        if (fixtureSetupAttr != null && fixtureSetup == null)
        {
          fixtureSetup = mi;
        }

        var fixtureTeardownAttr = (TestFixtureTeardownAttribute)Attribute.GetCustomAttribute(mi, typeof(TestFixtureTeardownAttribute));
        if (fixtureTeardownAttr != null && fixtureTeardown == null)
        {
          fixtureTeardown = mi;
        }

        var testSetupAttr = (TestSetupAttribute)Attribute.GetCustomAttribute(mi, typeof(TestSetupAttribute));
        if (testSetupAttr != null && testSetup == null)
        {
          testSetup = mi;
        }

        var testTeardownAttr = (TestTeardownAttribute)Attribute.GetCustomAttribute(mi, typeof(TestTeardownAttribute));
        if (testTeardownAttr != null && testTeardown == null)
        {
          testTeardown = mi;

        }

        var testAttr = (TestAttribute)Attribute.GetCustomAttribute(mi, typeof(TestAttribute));
        if (testAttr != null)
        {
          tests.Add(mi);
        }
      }

      return tests;
    }

    private void RunTestFixtureSetup(MethodInfo testFixtureSetup)
    {
      var testType = CurrentTestClass.GetType();
      if (testFixtureSetup != null)
      {
        testFixtureSetup.Invoke(CurrentTestClass, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);
      }
    }

    private void RunTestSetup(MethodInfo testSetup)
    {
      var testType = CurrentTestClass.GetType();

      if (testSetup != null)
      {
        testSetup.Invoke(CurrentTestClass, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);

      }
    }

    private void RunTest(MemberInfo test)
    {
      var testType = CurrentTestClass.GetType();
      var currentTestAttr = (TestAttribute)Attribute.GetCustomAttribute(test, typeof(TestAttribute));
      var expectedException = (TestExpectedException)Attribute.GetCustomAttribute(test, typeof(TestExpectedException));
      var isDestructive = currentTestAttr == null ? false : currentTestAttr.Options.HasFlag(TestOptions.Destructive);
      var ignore = currentTestAttr == null ? false : currentTestAttr.Ignore;
      var isProductionEnv = SiteContext.ServerLocation == ServerLocationType.Ote ||
                            SiteContext.ServerLocation == ServerLocationType.Prod;

      var canRunInProd = (!isDestructive && 
                         (currentTestAttr == null ? false : currentTestAttr.Options.HasFlag(TestOptions.Production)) && 
                         isProductionEnv);

      try
      {

        if (ignore)
        {
          AddTestResult(null, test.Name, "IGNORE - This test was flagged to be ignored by the test suite runner.");
        }
        else if ((!RunDestructiveTests && isDestructive) || (RunDestructiveTests && !canRunInProd))
        {
          AddTestResult(null, test.Name, "DESTRUCTIVE - This test was flagged as destructive and the test suite is not set to run destructive tests or the enviornment you are trying to execute this test in is not allowed.");
        }
        else
        {
          if (!isProductionEnv || canRunInProd)
          {
            ((MethodInfo)test).Invoke(CurrentTestClass, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);
            AddTestResult(true, test.Name, "Test completed successfully.");
          }
        }

      }
      catch (Exception testException)
      {
        if (expectedException != null && testException.InnerException != null &&
            expectedException.ExceptionType == testException.InnerException.GetType())
        {

          AddTestResult(true, test.Name, testException.InnerException.Message);

        }
        else
        {
          AddTestResult(false, test.Name,
                        testException.InnerException != null
                          ? testException.InnerException.Message
                          : testException.Message);
        }

      }
    }

    private void RunTestTeardown(MethodInfo testTeardown)
    {
      var testType = CurrentTestClass.GetType();
      if (testTeardown != null)
      {
        testTeardown.Invoke(CurrentTestClass, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);
      }
    }

    private void RunTestFixtureTeardown(MethodInfo testFixtureTeardown)
    {
      var testType = CurrentTestClass.GetType();
      if (testFixtureTeardown != null)
      {
        testFixtureTeardown.Invoke(CurrentTestClass, BindingFlags.Default | BindingFlags.InvokeMethod, null, new object[] { }, null);
      }
    }

    #endregion


  }

}
